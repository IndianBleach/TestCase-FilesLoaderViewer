
## Начало работы
1) Необходимо создать базу FileX (пустую). В разработке использовалась MSSQL.                                                                                           
2) Изменить конфигурацию сервера БД в appsettings.json
3) Api документация представлена ниже. Работа с запросами проводилась через Postman
 
#### HealthCheck
По первому роуту api установлен HealthCheck, который проверяет состояние коннекта с БД.
 
## API docs.
#### Посмотреть все файлы
```
https://localhost:7145/api/files/all
```
*Method: GET*


#### Загрузить файл
```
https://localhost:7145/api/files/upload
```
*Method: POST*                                                                                                                                                        
*Body: {file}*                                                                                                                                                        
*form-data*                                                                                                                                                             
Файл передаётся через body, название в body обязательно должно быть file, тип: form-data


#### Скачать файл
```
https://localhost:7145/api/files/download/{fileId}
Пример: https://localhost:7145/api/files/download/e6dd6f3f-6742-4d60-a1df-d561fb2fb509KL
```
*Method: GET*


#### Скачать файл по одноразовой ссылке
```
https://localhost:7145/api/files/l/{уникальная часть ссылки}
Пример: https://localhost:7145/api/files/l/lmMcXf7UQgaI6SO9
```
*Method: GET*


#### Создать и получить одноразовую ссылку для файла
```
https://localhost:7145/api/files/getlink?file={fileId}
Пример: https://localhost:7145/api/files/getlink?file=40fa7d51-ef82-4fde-8b21-8b381b996fb5
```
*Method: POST*

## Одновременная загрузка файлов
На клиенте каждый файл закинутый в форму отправлялся отдельным асинхронным запросом, где под загрузку файла создавалась отдельная задача. В итоге получалась стопка асинхронных запросов с ответов от сервера, что файл загружен. Проверка происходила след. образом: 
1) от <input multiple file/> через цикл отправлялись запросы в на сервер и обрабатывался результат с уведомлением пользователя. 
2) в Postman создавалось несколько одинаковых запросов, где загружались файлы разного объёма, одноврменно отправлялись, а после приходил ответ (ответ от сервера приходил не в порядке загрузки файлов, а соответственно в зависимости от объёма загружаемого файла).

