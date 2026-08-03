import json
import urllib.request
import sys
import os

# Configuración del entorno local
# Usamos variables de entorno con valores por defecto para mayor flexibilidad
OLLAMA_URL = os.getenv("OLLAMA_URL", "http://127.0.0.1:11434/api/generate")
OLLAMA_MODEL = os.getenv("OLLAMA_MODEL", "llama3") 
DIFF_FILE = "pr_diff.txt"
OUTPUT_FILE = "ai_response.json"

def clean_diff(diff_text):
    """Limpia el diff de Git para ahorrar tokens y mejorar el contexto del LLM."""
    cleaned_lines = []
    for line in diff_text.split('\n'):
        # Filtrar metadatos que no aportan valor semántico al análisis
        if line.startswith('index ') or line.startswith('similarity index') or line.startswith('diff --git'):
            continue
        cleaned_lines.append(line)
    
    # Opcional: Limitar la longitud máxima para evitar desbordar el contexto del modelo
    max_chars = 10000 
    cleaned_text = '\n'.join(cleaned_lines)
    return cleaned_text[:max_chars]

def main():
    if not os.path.exists(DIFF_FILE):
        print(f"Error: No se encontró el archivo {DIFF_FILE}. Verifica el paso de Git diff.")
        sys.exit(1)

    with open(DIFF_FILE, 'r', encoding='utf-8') as f:
        raw_diff = f.read()

    # Si el diff está vacío, no hay necesidad de llamar a la IA
    if not raw_diff.strip():
        print("El diff está vacío. No hay cambios para analizar.")
        with open(OUTPUT_FILE, 'w', encoding='utf-8') as out:
            json.dump({"response": "No se detectaron cambios sustanciales en el código para revisar."}, out)
        sys.exit(0)

    diff_limpio = clean_diff(raw_diff)

    # Prompt estructurado
    system_prompt = f"""
    You are a Senior DevOps Engineer and Tech Lead. Your task is to perform a strict and constructive code review on the following Pull Request Git Diff.

    ### REVIEW OBJECTIVES:
    1. **Security & Credentials:** Detect exposed secrets (tokens, passwords, API keys) and vulnerabilities (SQL injections, insecure dependencies).
    2. **Infrastructure Practices:** Validate that Docker files, CI/CD pipelines, or network configurations follow the principle of least privilege, cache optimization, and image size reduction.
    3. **Code Quality:** Identify poor development practices, paying special attention to stacks like Laravel, React Native, or automation scripts. Evaluate performance and maintainability.

    ### RESPONSE RULES (MANDATORY):
    - DO NOT greet or provide introductions. Start directly with the analysis.
    - Use Markdown format EXCLUSIVELY.
    - Structure your response using these three mandatory headings: `## 🚨 Critical Findings`, `## 💡 Improvement Suggestions`, and `## ✅ What is Good`.
    - If the code has no issues, indicate it briefly under the corresponding headings.
    - If you suggest a change, include a short code block with the solution.
    - Be direct and concise.

    ### CODE TO REVIEW:
    {diff_limpio}
    """

    payload = {
        "model": OLLAMA_MODEL,
        "prompt": system_prompt,
        "stream": False
    }

    data = json.dumps(payload).encode('utf-8')
    req = urllib.request.Request(
        OLLAMA_URL, 
        data=data, 
        headers={'Content-Type': 'application/json'}
    )

    print(f"Enviando el análisis estructurado a Ollama ({OLLAMA_MODEL})...")
    
    try:
        with urllib.request.urlopen(req, timeout=600) as response:
            result = json.loads(response.read().decode('utf-8'))
            ai_text = result.get("response", "La IA no devolvió ninguna respuesta.")
            
            # Guardamos la respuesta en un JSON limpio para el bot de GitHub
            with open(OUTPUT_FILE, 'w', encoding='utf-8') as out:
                json.dump({"response": ai_text}, out)
                
        print(f"✅ Análisis completado con éxito. Guardado en {OUTPUT_FILE}.")
    
    except Exception as e:
        print(f"❌ Error al conectar con Ollama en {OLLAMA_URL}: {e}")
        # En caso de fallo de IA, no rompemos el pipeline, solo avisamos
        with open(OUTPUT_FILE, 'w', encoding='utf-8') as out:
            json.dump({"response": f"⚠️ **Fallo en la validación de IA:** No se pudo contactar al servidor local de Ollama. Error: {e}"}, out)

if __name__ == "__main__":
    main()