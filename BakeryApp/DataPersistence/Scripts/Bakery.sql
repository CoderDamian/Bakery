-- ================================================================
-- Creacion de tablas, claves primarias, secundarias, restricciones
-- Aplicacion: Bakery
-- Esquema: CIA
-- Fecha actualizacion: 2023.02.24
-- =================================================================

REM *************************************************************
REM Create objects to hold users information for authentications.
REM *************************************************************

Prompt *** Creating USER table ***
CREATE TABLE users (
    id       NUMBER(3),
    username VARCHAR2(5),
    password VARCHAR2(10)
);

alter table users 
add RefreshToken nvarchar2(100) null;

alter table users
add RefreshTokenExpiryTime  date null;

prompt - creating constraints
ALTER TABLE users ADD CONSTRAINT user_pk PRIMARY KEY ( id );

prompt - creating sequences
CREATE SEQUENCE user_seq_01 START WITH 1 INCREMENT BY 1;

prompt - creating triggers
CREATE OR REPLACE TRIGGER user_tr_01 BEFORE
    INSERT ON users
    FOR EACH ROW
BEGIN
    :new.id := user_seq_01.nextval;
END;

REM ********************************************
REM Create object to hold products information.
REM ********************************************

prompt *** Creating PRODUCT table
CREATE TABLE products (
    id          NUMBER(3),
    name        VARCHAR2(50) NOT NULL,
    description VARCHAR2(150) NOT NULL,
    price       NUMBER(3, 2),
    imagename   VARCHAR2(50)
);

prompt - creating constraint
ALTER TABLE products ADD CONSTRAINT product_pk PRIMARY KEY ( id );

prompt - creating sequence
CREATE SEQUENCE product_seq_01 START WITH 1 INCREMENT BY 1;

prompt - creating trigger
CREATE OR REPLACE TRIGGER product_tr_01 BEFORE
    INSERT ON products
    FOR EACH ROW
BEGIN
    :new.id := product_seq_01.nextval;
END;

REM ********************************
REM Inserting data in PRODUCT table
REM ********************************
insert into products (name, description, price, imagename)
with t as (
    select 'Carrot Cake', 'A scrumptious mini-carrot cake encrusted with sliced almonds', 8.99, 'carrot_cake.jpg' from dual union all
    select 'Lemon Tart', 'A delicious lemon tart with fresh meringue cooked to perfection', 9.99, 'lemon_tart.jpg' from dual union all
    select 'Cupcakes', 'Delectable vanilla and chocolate cupcakes', 5.99, 'cupcakes.jpg' from dual union all
    select 'Bread', 'Fresh baked French-style bread', 1.49, 'bread.jpg' from dual union all
    select 'Pear Tart', 'A glazed pear tart topped with sliced almonds and a dash of cinnamon', 5.99, 'pear_tart.jpg' from dual union all
    select 'Chocolate Cake', 'Rich chocolate frosting cover this chocolate lover''s dream', 8.99, 'chocolate_cake.jpg' from dual
    )
select * from t;

REM *****************************
REM Inserting data in USER table
REM *****************************
insert into users (username, password) values ('user1', '111');
insert into users (username, password) values ('user2', '222');


REM *******************
REM Creating procedures
REM *******************
-- Procedure to update token values by userID
CREATE OR REPLACE procedure UpdateUserToken(userId in number, refreshToken in nvarchar2, expiryTime Date) 
AS
BEGIN

    UPDATE users
    SET
        refreshtoken = updateusertoken.refreshtoken,
        refreshtokenexpirytime = updateusertoken.expirytime
    WHERE
        id = updateusertoken.userid;

END updateusertoken;
