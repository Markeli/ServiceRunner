# ServiceRunner
Утилита, которая позволяет другие приложения как сервис Windows

## Реализация

Утилита для управления сервисами использует TopShelf. ServiceRunner поддерживает большинство команд TopShelf. TopShelf позволяет из любого приложения .Net сделать Windows сервис, однако для уже скомпилированных сторонних приложений она не подходит. Зато ServiceRunner подходит.

## Конфигурация

Для запуска приложения как сервиса необходимо создать конфигурационный файл (это json файл):

```json
{
  "ServiceName": "OSRM control service",  
  "ServiceSystemName": "OSRMCS",
  "ServiceDescription": "Service to control OSRM engine, start, stop, restart on crashes",
  "ServicePath": "d:\\Temp\\ChildProcessTest.exe",  
  "ServiceArguments": "",  
  "RestartTimeout": 0,  
  "RestartAfterCrash": "true",  
  "RestartCountOnFail": 3
}
``` 
Название параметра| Описание
------------ | -------------
ServiceName | название сервиса
ServiceSystemName | название сервиса, которое будет отображаться в списке процессов
ServiceDescription | описание сервиса
ServicePath | путь к запускаемому файлу
ServiceArguments | аргументы, с которым нужно запускать сервис
RestartTimeout | необходимо ли перезапускать сервис после ошибки
RestartAfterCrash | Таймаут в минутах перезапуска сервиса после падения
RestartCountOnFail | Количество попыток перезапустить сервис после ошибки

## Usage
ServiceRunner  -service serviceName [verb] [-option:value] 
	
    -help             Выводит подсказку

    -service            (Обязателен) Путь к файлу конфигурации сервиса или название сервиса, 
						если конфигурация находится в стандартном каталоге (путь к каталогу задается в ServiceRunner.exe.config) 
	
    run                 Запускает сервис как консольное приложение (по умолчанию)

    install             Региструриует сервис как службу Windows

      --autostart       Запуск службы автоматически
      --disabled        Служба отключена
      --manual          Запуск службы вручную
      --delayed         Запуск службы с задержкой 
      -instance         Название сервиса, который может устанавливаться много раз   
	  
	  -username         Имя пользователя, от имени которого будет запущен сервис
      -password         Пароль пользователя, от имени которого будет запущен сервис
      --localsystem     Запускает сервис от имени локального системного аккаунта (local system account)
      --localservice    Запускает сервис от имени локального сервсиного аккаунта (local service account)
      --networkservice  Запускает сервис с правами network service 
      --interactive     При установке будут запрошены логин и пароль пользователя, от имени которого будет запущен сервис

    start               Запускает сервис, если он еще не запущен
      
    stop                Останаливаетс сервис, если он запущен

    uninstall           Удаляет сервис из служб Windows

	--sudo				Позволяет выполнить команду от имени админстратора

Примеры:

    service install -service:osrm
        Регистрирует сервис по его имени (конфигурационный файл находится в каталоге для файлов конфигурация)

		
    service install -service:'c:/temp/osrm.service'
        Регистрирует сервис по конфигурационному файлу, к которому указан путь
		
    service uninstall -service:osrm
        Разрегистрирует сервис

    service install -instance:001 -service:osrm
        Регистрирует сервис с заданным именем