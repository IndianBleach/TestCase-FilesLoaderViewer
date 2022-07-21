# TestCase-FilesLoaderViewer
 
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

