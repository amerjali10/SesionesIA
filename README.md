# Ahorcado en consola

Primer proyecto del curso **Prompts de Copilot para Git/GitHub en .NET**.
El equipo construye un juego de ahorcado completo en C#, con GitHub Copilot como asistente,
y comparte su trabajo por GitHub en cada cambio de piloto.

## Integrantes

Las fichas del equipo están en la carpeta [`equipo`](equipo/).

## Cómo ejecutar el juego

1. Abre una terminal en la carpeta del repositorio.
2. Ejecuta:

   ```powershell
   dotnet run
   ```

## Cómo trabajamos

| Momento | Qué hace el equipo |
| --- | --- |
| Antes de escribir | `git pull` para traer lo último del equipo |
| Durante el turno | El piloto escribe, el navegante redacta los prompts y revisa |
| Al cambiar de piloto | Quien sale: `git add .`, `git commit -m "..."` y `git push`. Quien entra: `git pull` |

## Lo que debe hacer el juego al final del módulo

- [ ] Elegir una palabra al azar de una lista
- [ ] Mostrar la palabra oculta con guiones bajos
- [ ] Pedir una letra y validar que sea una sola letra que no se haya usado
- [ ] Revelar las letras acertadas y contar los errores
- [ ] Dibujar el ahorcado en ASCII según los errores
- [ ] Terminar cuando se adivina la palabra o se agotan los intentos
- [ ] Calcular el puntaje y preguntar si se quiere jugar otra vez
