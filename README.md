acumatica
=========

mailer tester
Автоматизированный браузерный тест web-интерфейса mail.ru, использующий Selenium.
В выбранном браузере открывает указанный почтовый  сервис.
Входит с указанным логином и паролем.
Отправляет пустое письмо "себе".
Входит повторно и проверяет наличие письма от "себя"

Параметры appSettings:
browser - "chrome", "safari", "firefox", "ie"
mailerAddress - адрес в формате mail.ru (без схемы) 
username - имя пользователя
password - пароль. Если установлена пустая строка, пароль можно будет ввести во время выполнения.
isLogEnabled - включение/отключение вывода в файл (WebMail.log).
