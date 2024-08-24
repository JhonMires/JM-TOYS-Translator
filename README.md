# JM-TOYS-Translator

![Revit API](https://img.shields.io/badge/Revit%20API%202022-blue.svg)
![Platform](https://img.shields.io/badge/platform-Windows-lightgray.svg)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

JM-TOYS-Translator es una herramienta que permite generar traducciones utilizando la API de DeepL. Este proyecto está diseñado para ser usado con la API de Revit 2022, y está enfocado en mejorar la productividad dentro de entornos BIM.

## ¡Únete a la Comunidad!

Este proyecto es de código abierto y está abierto a contribuciones de la comunidad. ¡Cualquier funcionalidad que desees agregar es bienvenida! Además, si te apasiona la programación y la innovación en BIM, te invitamos a unirte al grupo más grande y mejor de programación e innovación en BIM en WhatsApp: [Únete aquí](https://chat.whatsapp.com/FGR6cGRrgLe9RtXQ5Gl4EE).

Recuerda que también agradeceremos que le des un **like** al repositorio para apoyar el desarrollo de esta herramienta.

## Instalación

Para usar este proyecto, es necesario que generes un archivo de tipo ADDIN. Si no sabes cómo hacerlo, puedes seguir las instrucciones detalladas en el siguiente enlace: [Addin Manifest and Guide]
(https://thebuildingcoder.typepad.com/blog/2010/04/addin-manifest-and-guidize.html).

![WhatsApp Video 2024-08-23 at 7](https://github.com/user-attachments/assets/47d19928-b4c8-4a93-9164-aeeabfcfa094)


## Importante

Antes de empezar a usar la herramienta, asegúrate de configurar tu clave API de DeepL. Para ello, sigue estos pasos:

1. Abre la clase `Cache`.
2. Busca la propiedad `apikey`.
3. Reemplaza el valor por tu propia clave API de DeepL.

```csharp
public class Cache
{
    // Otros métodos y propiedades...

    public static string apikey = "Reemplaza con tu clave API"; // Reemplaza con tu clave API
}
```

## Contribuciones

Las contribuciones son bienvenidas. Si tienes ideas para nuevas funcionalidades, correcciones de errores, o mejoras en la documentación, no dudes en hacer un pull request. Todos los comentarios y sugerencias serán tomados en cuenta para mejorar la herramienta.

## Licencia
Este proyecto está licenciado bajo la Licencia MIT. Puedes ver más detalles en el archivo LICENSE.


