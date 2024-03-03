# Servicio de Noticias RSS

Este proyecto consiste en un servicio de backend desarrollado con ASP.NET Core que proporciona noticias obtenidas de fuentes RSS. Permite a los clientes realizar solicitudes para obtener noticias de una fuente RSS específica.

## Funcionalidades

- **Obtener noticias desde RSS:** Los clientes pueden enviar una solicitud GET con la URL de la fuente RSS para recibir una lista de noticias recientes.
- **Límite de solicitudes:** Se implementa un límite en el número de solicitudes que un cliente puede realizar en un período de tiempo determinado para evitar la sobrecarga del servidor.

## Tecnologías Utilizadas

- ASP.NET Core: Para el desarrollo del servicio web.
- System.ServiceModel.Syndication: Para el procesamiento de fuentes RSS.
- LINQ: Para la manipulación de colecciones de datos.
- Microsoft.AspNetCore.Mvc: Para la creación de controladores y rutas.

## Instalación

1. Clona este repositorio en tu máquina local.
2. Abre el proyecto en Visual Studio o cualquier otro IDE compatible con ASP.NET Core.
3. Ejecuta la aplicación para iniciar el servidor.

## Uso

- Después de iniciar el servidor, puedes realizar solicitudes GET a la ruta `/api/news` proporcionando la URL de la fuente RSS como parámetro de consulta.
- Por ejemplo: `GET /api/news?rssUrl=https://example.com/rss`.
- El servidor responderá con una lista de noticias en formato JSON si la solicitud es válida.

## Contribución

Si quieres contribuir a este proyecto, por favor sigue los siguientes pasos:

1. Haz un fork del repositorio.
2. Crea una nueva rama (`git checkout -b feature/nueva-funcionalidad`).
3. Realiza tus cambios y haz commit de ellos (`git commit -am 'Añade nueva funcionalidad'`).
4. Haz push de la rama (`git push origin feature/nueva-funcionalidad`).
5. Abre un pull request.

## Licencia

Este proyecto está bajo la Licencia MIT. Consulta el archivo [LICENSE](LICENSE) para más detalles.
