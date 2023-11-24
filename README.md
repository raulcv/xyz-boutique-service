## Projecto Rest API Con .net Core 7 y Entity Framerork

Este projecto es un servicio API Rest está documentado en Swagger para poder ver a detalle los EndPoints. La arquitectura está basado en Repositorio con inyeccion de depenecnias. Además consta de 4 proyectos para poder separar y hacer más modular el proyecto. 

------------
##### Corre el proyecto en tu máquina local

Necesitas `dotnet CLI` check dotnet version `dotnet --version` o usa el IDE `Visual Studio` 


1: Clone with git.
```
git clone git@github.com:raulcv/xyz-boutique-service.git
```

2: Located in the project, Install dotnet project dependencies with dotnet CLI
```
dotnet restore 
```

Ejecutar los scripts 'xyzboutique.sql' de SQL para poder ejecutar el proyecto y consumir los END PONITS.

Necesitas SQLServer o puedes crear una instancia de Docker.

Por último añade a tu variable de entorno la variable XYZBOUTIQUEDB='sql-connection-string' conta conección a sql server.

Finalmente ejecuta la solución del proyecto `xyzboutique.api`

 ```
 dotnet run --project meetingplanner.app
 ```


Adjunto un archivo xyz.postman_collection.json de postman para que lo importes y ejecutes los endpoints ya preparados si no desea usar Swagger.










------------------------------------------------------------------------
<p align="center">
	With :heart: by <a href="https://www.raulcv.com" target="_blank">raulcv</a>
</p>

#
<h3 align="center">🤗 If you found helpful this repo, let me a star 🐣</h3>
<p align="center">
<a href="https://www.buymeacoffee.com/iraulcv" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/default-orange.png" alt="Buy Me A Coffee" height="41" width="174"></a>
</p>