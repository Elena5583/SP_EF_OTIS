CREATE TABLE Clients
(
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    FirstName CHARACTER VARYING(50),
    LastName CHARACTER VARYING(150),
    Inn CHARACTER VARYING(12),
    Age INTEGER,
    Address CHARACTER VARYING(500) NOT NULL DEFAULT 'Неизвестно'
);

CREATE TABLE Currencies
(
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    Code CHARACTER VARYING(50),
    ShortName CHARACTER VARYING(50) NULL,
    Name CHARACTER VARYING(150)
);

CREATE TABLE BankAccounts
(
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    ClientId UUID,
    CurrencyId UUID,
    BankAccountTurnover money,
    NumberAccount CHARACTER VARYING(20),
    FOREIGN KEY (CurrencyId) REFERENCES Currencies (Id) ON DELETE SET NULL,
    FOREIGN KEY (ClientId) REFERENCES Clients (Id) ON DELETE CASCADE
);

INSERT INTO Currencies  (Id, Code, ShortName, Name)
VALUES
('ca4b2560-5da6-43b6-a8b0-88f905f4bcad', '643', 'RUB', 'Рубль'),
('89f686ec-1900-4a36-85f0-119083f9b76d', '840', 'USD', 'Доллар'),
('6bad1550-be18-443d-9ea8-9546a67335fa', '978', 'EUR', 'Евро'),
('a816fdb8-84e6-47e2-860d-01f6c688cd56', '036', 'AUD', 'Австраллийский доллар'),
('d93b419a-ebf6-4a81-be5d-356de7e20aae', '980', 'UAH', 'Гривна');

INSERT INTO Clients  (Id, FirstName, LastName, Inn, Age)
VALUES
('19642e94-1a03-40d2-93d4-9bd3109c23a9', 'Юрий', 'Кононенко', '235102761632', '56'),
('ecdcffd5-f8ac-44c6-bff1-ecf248585d74', 'Вероника', 'Нестеренко', '462100050262', '39'),
('2bc341db-e83c-45ec-9464-ec3402a56aee', 'Сергей', 'Рыбин', '926727043446', '49'),
('2482d478-a2b8-48c2-a3dc-62c73e69a056', 'Виталий', 'Дремин', '926259927200', '23'),
('7b2618a6-5d53-4a2a-b358-107690a8ac1f', 'Тамила', 'Юсупова', '921206388080', '45');

INSERT INTO BankAccounts  (ClientId, CurrencyId, BankAccountTurnover, NumberAccount)
VALUES
('19642e94-1a03-40d2-93d4-9bd3109c23a9', 'ca4b2560-5da6-43b6-a8b0-88f905f4bcad', 6000, '47423810613230002855'),
('ecdcffd5-f8ac-44c6-bff1-ecf248585d74', 'ca4b2560-5da6-43b6-a8b0-88f905f4bcad', 5555.55, '40702810163020000607'),
('2bc341db-e83c-45ec-9464-ec3402a56aee', 'ca4b2560-5da6-43b6-a8b0-88f905f4bcad', 1234567.45, '45918810375090000023'),
('2482d478-a2b8-48c2-a3dc-62c73e69a056', 'ca4b2560-5da6-43b6-a8b0-88f905f4bcad', 66666666, '47444810611070000005'),
('7b2618a6-5d53-4a2a-b358-107690a8ac1f', '89f686ec-1900-4a36-85f0-119083f9b76d', 111111, '40702810367150000306');