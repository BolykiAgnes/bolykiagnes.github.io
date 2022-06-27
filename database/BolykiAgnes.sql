/*Táblák létrehozása, feltöltése*/
create table Felhasznalok(
felhasznalo_id number(2) not null,
nev varchar2(50) not null,
lakcim varchar2(50) not null,
telefonszam varchar2(11) not null,
email varchar2(50) not null,
constraint f_pk primary key (felhasznalo_id)
);

create table Futar(
futar_id number(1) not null,
felhasznalo_id number(2),
fizetes number(6),
constraint fu_pk primary key (futar_id),
constraint fu_fk foreign key (felhasznalo_id) references Felhasznalok(felhasznalo_id) on delete cascade
);

create table Rendelesek(
rendeles_id number(2) not null,
felhasznalo_id number(2) not null,
futar_id number(1),
rendeles_időpontja date not null,
kezbesites_idopontja date,
constraint r_pk primary key (rendeles_id),
constraint r_fk1 foreign key (felhasznalo_id) references Felhasznalok(felhasznalo_id) on delete cascade,
constraint r_fk2 foreign key (futar_id) references Futar(futar_id) on delete cascade
);

create table Termekek(
termek_id number(2) not null,
tipus number(1) not null,
megnevezes varchar2(50) not null,
ar number(4) not null,
constraint t_pk primary key (termek_id),
constraint t_ck check(tipus between 0 and 1)
);

create table Ertekelesek(
ertekeles_id number(2) not null,
felhasznalo_id number(2) not null,
pontszam number(1) not null,
szoveges_velemeny varchar2(1000),
constraint e_pk primary key (ertekeles_id),
constraint E_fk foreign key (felhasznalo_id) references Felhasznalok(felhasznalo_id) on delete cascade
);

create table rendelesXtermekek(
rendeles_id number(2) not null,
termek_id number(2) not null,
mennyiseg number(3) not null,
constraint rxt_fk1 foreign key (rendeles_id) references Rendelesek(rendeles_id) on delete cascade,
constraint rxt_fk2 foreign key (termek_id) references Termekek(termek_id) on delete cascade,
constraint rxt_pk primary key (rendeles_id, termek_id)
);

insert into Felhasznalok values(1,'Molnár Péter','1108 Liliom utca 16',+36399118490,'molnar.peter@hotmail.com');
insert into Felhasznalok values(2,'Kiss Viktória','1029 Liliom utca 16',+36225099928,'kiss.viktoria@outlook.com');
insert into Felhasznalok values(3,'Katona Zsolt','1183 Liliom utca 16',+36888961784,'katona.zsolt@icloud.com');
insert into Felhasznalok values(4,'Fekete Dávid','1054 Bodnár utca 84',+36719007858,'fekete.david@hotmail.com');
insert into Felhasznalok values(5,'Juhász Tamás','1108 Ecetfa utca 42',+36195611125,'juhasz.tamas@hotmail.com');
insert into Felhasznalok values(6,'Tóth Viktória','1135 Napfény utca 89',+36875782560,'toth.viktoria@citromail.hu');
insert into Felhasznalok values(7,'Molnár Nóra','1122 Liliom utca 16',+36955276742,'molnar.nora@outlook.com');
insert into Felhasznalok values(8,'Kovács László','1151 Álmos utca 14',+36658213607,'kovacs.laszlo@icloud.com');
insert into Felhasznalok values(9,'Fehér Dávid','1151 Nádasdy utca 76',+36982167208,'feher.david@outlook.com');
insert into Felhasznalok values(10,'Kovács Nóra','1172 Napfény utca 89',+36576769948,'kovacs.nora@freemail.hu');
insert into Felhasznalok values(11,'Varga Balázs','1162 Hegyi utca 8',+36876692545,'varga.balazs@outlook.com');
insert into Felhasznalok values(12,'Molnár Anikó','1081 Gomb utca 53',+36781262500,'molnar.aniko@icloud.com');
insert into Felhasznalok values(13,'Fehér Anita','1122 Napfény utca 89',+36903412202,'feher.anita@uni-obuda.hu');
insert into Felhasznalok values(14,'Gál Tamás','1213 Ady Endre utca 12',+36163763790,'gal.tamas@uni-obuda.hu');
insert into Felhasznalok values(15,'Fekete Tamás','1097 Bölény utca 2',+36780271556,'fekete.tamas@outlook.com');
insert into Felhasznalok values(16,'Katona László','1097 Bodnár utca 84',+36662235934,'katona.laszlo@icloud.com');
insert into Felhasznalok values(17,'Nagy András','1172 Kökény utca 6',+36070794244,'nagy.andras@citromail.hu');
insert into Felhasznalok values(18,'Török Eszter','1122 Gomb utca 53',+36041609343,'torok.eszter@freemail.hu');
insert into Felhasznalok values(19,'Nagy Anikó','1213 Liliom utca 16',+36322645488,'nagy.aniko@outlook.com');
insert into Felhasznalok values(20,'Kovács Nóra','1116 Ecetfa utca 42',+36155602306,'kovacs.nora@uni-obuda.hu');

insert into Futar values(1,9,117021);
insert into Futar values(2,6,106785);
insert into Futar values(3,1,92984);

insert into Rendelesek values(1,5,1,to_date('2019. 10. 3.  07:08:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 03. 7:49:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(2,4,2,to_date('2019. 10. 20.  15:48:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 20. 16:29:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(3,10,3,to_date('2019. 10. 6.  06:01:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 06. 6:46:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(4,17,3,to_date('2019. 10. 12.  15:41:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 12. 16:32:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(5,19,2,to_date('2019. 10. 16.  19:35:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 16. 20:32:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(6,7,1,to_date('2019. 10. 1.  01:07:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 01. 2:01:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(7,16,1,to_date('2019. 10. 1.  01:07:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 01. 1:49:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(8,20,2,to_date('2019. 10. 6.  22:02:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 06. 22:36:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(9,4,1,to_date('2019. 10. 9.  11:48:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 09. 12:35:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(10,11,2,to_date('2019. 10. 8.  05:08:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 08. 6:07:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(11,3,3,to_date('2019. 10. 2.  08:14:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 02. 8:50:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(12,15,3,to_date('2019. 10. 22.  23:00:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 22. 23:44:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(13,2,1,to_date('2019. 10. 1.  16:20:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 01. 17:01:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(14,15,3,to_date('2019. 10. 21.  05:20:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 21. 6:05:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(15,6,1,to_date('2019. 10. 8.  16:02:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 08. 17:01:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(16,7,2,to_date('2019. 10. 15.  15:40:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 15. 16:39:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(17,10,2,to_date('2019. 10. 7.  23:08:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 08. 0:05:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(18,20,3,to_date('2019. 10. 7.  23:08:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 07. 23:58:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(19,18,2,to_date('2019. 10. 5.  02:15:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 05. 2:53:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(20,12,2,to_date('2019. 10. 2.  08:14:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 02. 9:02:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(21,11,3,to_date('2019. 10. 11.  06:26:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 11. 7:20:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(22,3,3,to_date('2019. 10. 8.  16:02:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 08. 16:55:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(23,3,3,to_date('2019. 10. 4.  23:31:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 05. 0:07:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(24,1,3,to_date('2019. 10. 22.  23:00:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 22. 23:42:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(25,5,3,to_date('2019. 10. 8.  05:08:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 08. 6:01:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(26,10,1,to_date('2019. 10. 7.  10:55:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 07. 11:35:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(27,18,1,to_date('2019. 10. 16.  18:50:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 16. 19:24:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(28,7,2,to_date('2019. 10. 1.  16:20:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 01. 16:52:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(29,10,3,to_date('2019. 10. 5.  02:15:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 05. 3:12:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(30,7,3,to_date('2019. 10. 16.  18:50:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 16. 19:38:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(31,11,1,to_date('2019. 10. 17.  01:26:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 17. 2:13:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(32,6,3,to_date('2019. 10. 7.  23:40:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 08. 0:40:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(33,15,3,to_date('2019. 10. 22.  18:45:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 22. 19:20:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(34,8,2,to_date('2019. 10. 9.  11:48:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 09. 12:42:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(35,6,3,to_date('2019. 10. 13.  19:31:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 13. 20:17:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(36,5,3,to_date('2019. 10. 1.  01:07:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 01. 2:06:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(37,13,3,to_date('2019. 10. 4.  23:31:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 05. 0:21:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(38,6,1,to_date('2019. 10. 12.  15:41:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 12. 16:16:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(39,2,2,to_date('2019. 10. 16.  18:50:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 16. 19:33:00','YYYY-MM-DD HH24:MI:SS'));
insert into Rendelesek values(40,10,2,to_date('2019. 10. 4.  21:23:00','YYYY-MM-DD HH24:MI:SS'),to_date('2019. 10. 04. 21:57:00','YYYY-MM-DD HH24:MI:SS'));

insert into Termekek values(1,0,'sajttal töltött rántott csirkemell',1746);
insert into Termekek values(2,0,'bakonyi sertésborda',1876);
insert into Termekek values(3,0,'citromos-fokhagymás csirkecomb',2210);
insert into Termekek values(4,0,'zöldséges csirkecombfilé',2147);
insert into Termekek values(5,0,'dupla sajtos csirketekercs',2417);
insert into Termekek values(6,0,'töltött padlizsán',2389);
insert into Termekek values(7,0,'szilvás gombóc',2328);
insert into Termekek values(8,0,'kenyérlángos',1514);
insert into Termekek values(9,0,'mozzarellával töltött tökfasírt',2491);
insert into Termekek values(10,0,'fűszeres tarja',1823);
insert into Termekek values(11,0,'grilltál',1565);
insert into Termekek values(12,0,'fehérboros gombaragu',2308);
insert into Termekek values(13,0,'paprikás krumpli',2295);
insert into Termekek values(14,0,'murai szelet',1681);
insert into Termekek values(15,0,'szecsuáni csirkemell',1608);
insert into Termekek values(16,0,'sajtos csirkemell',1624);
insert into Termekek values(17,0,'sült hekk',2016);
insert into Termekek values(18,0,'lángos',1912);
insert into Termekek values(19,0,'őszibarackos csirkemell',1892);
insert into Termekek values(20,0,'rozmaringos sült csirke',2200);
insert into Termekek values(21,0,'húsos-zöldséges pizzatekercs',2398);
insert into Termekek values(22,0,'stefánia vagdalt',2340);
insert into Termekek values(23,0,'kijevi szezámmagos csirkegolyó',2206);
insert into Termekek values(24,0,'brokkolis sonkás makaróni',1582);
insert into Termekek values(25,0,'mustáros tejfölös csirke',1877);
insert into Termekek values(26,0,'sztrapacska',2047);
insert into Termekek values(27,0,'bolognai spagetti',2441);
insert into Termekek values(28,0,'túrógombóc',2387);
insert into Termekek values(29,0,'bakonyi sertésszelet',1879);
insert into Termekek values(30,0,'tejfölös gombapaprikás',1754);
insert into Termekek values(31,1,'coca-cola',350);
insert into Termekek values(32,1,'fanta',350);
insert into Termekek values(33,1,'gyömbér',350);
insert into Termekek values(34,1,'barackos tea',350);
insert into Termekek values(35,1,'citromos tea',350);

insert into Ertekelesek(ertekeles_id,felhasznalo_id,pontszam,szoveges_velemeny) values(1,3,5,'gyors volt a kiszállítás és a futár is kedves volt');
insert into Ertekelesek(ertekeles_id,felhasznalo_id,pontszam) values(2,16,5);
insert into Ertekelesek(ertekeles_id,felhasznalo_id,pontszam,szoveges_velemeny) values(3,7,2,'gyors a kiszállítás, viszont az étel már nem volt meleg');
insert into Ertekelesek(ertekeles_id,felhasznalo_id,pontszam) values(4,8,4);
insert into Ertekelesek(ertekeles_id,felhasznalo_id,pontszam,szoveges_velemeny) values(5,12,4,'az étel minősége kifogásolhatatlan, de a futár egy kicsit mogorva volt');
insert into Ertekelesek(ertekeles_id,felhasznalo_id,pontszam,szoveges_velemeny) values(6,9,5,'tökéletes volt minden!');
insert into Ertekelesek(ertekeles_id,felhasznalo_id,pontszam) values(7,10,5);
insert into Ertekelesek(ertekeles_id,felhasznalo_id,pontszam) values(8,1,4);
insert into Ertekelesek(ertekeles_id,felhasznalo_id,pontszam,szoveges_velemeny) values(9,5,5,'széles választék, gyors házhozszállítás');
insert into Ertekelesek(ertekeles_id,felhasznalo_id,pontszam) values(10,15,3);

insert into rendelesXtermekek values(1,35,2);
insert into rendelesXtermekek values(2,9,2);
insert into rendelesXtermekek values(3,5,1);
insert into rendelesXtermekek values(4,28,1);
insert into rendelesXtermekek values(5,23,1);
insert into rendelesXtermekek values(6,19,4);
insert into rendelesXtermekek values(7,15,4);
insert into rendelesXtermekek values(8,25,5);
insert into rendelesXtermekek values(9,11,4);
insert into rendelesXtermekek values(10,18,5);
insert into rendelesXtermekek values(11,10,5);
insert into rendelesXtermekek values(12,32,2);
insert into rendelesXtermekek values(13,1,4);
insert into rendelesXtermekek values(14,1,2);
insert into rendelesXtermekek values(15,33,1);
insert into rendelesXtermekek values(16,8,2);
insert into rendelesXtermekek values(17,29,5);
insert into rendelesXtermekek values(18,3,2);
insert into rendelesXtermekek values(19,7,1);
insert into rendelesXtermekek values(20,1,5);
insert into rendelesXtermekek values(21,31,5);
insert into rendelesXtermekek values(22,3,4);
insert into rendelesXtermekek values(23,18,4);
insert into rendelesXtermekek values(24,34,5);
insert into rendelesXtermekek values(25,30,2);
insert into rendelesXtermekek values(26,14,1);
insert into rendelesXtermekek values(27,9,1);
insert into rendelesXtermekek values(28,14,1);
insert into rendelesXtermekek values(29,14,5);
insert into rendelesXtermekek values(30,10,3);
insert into rendelesXtermekek values(31,2,3);
insert into rendelesXtermekek values(32,8,3);
insert into rendelesXtermekek values(33,31,1);
insert into rendelesXtermekek values(34,31,3);
insert into rendelesXtermekek values(35,25,2);
insert into rendelesXtermekek values(36,23,1);
insert into rendelesXtermekek values(37,27,3);
insert into rendelesXtermekek values(38,2,2);
insert into rendelesXtermekek values(39,32,1);
insert into rendelesXtermekek values(40,29,2);
insert into rendelesXtermekek values(31,1,2);
insert into rendelesXtermekek values(26,25,1);
insert into rendelesXtermekek values(5,29,3);
insert into rendelesXtermekek values(2,34,2);
insert into rendelesXtermekek values(37,21,5);
insert into rendelesXtermekek values(13,25,3);
insert into rendelesXtermekek values(37,12,4);
insert into rendelesXtermekek values(20,11,3);
insert into rendelesXtermekek values(26,9,2);
insert into rendelesXtermekek values(28,35,1);

/*Véglegesítünk*/
commit;

/*Egyszerű lekérdezések*/
select * from Termekek
where ar > 2000
order by ar desc;

select * from Felhasznalok
where nev like 'Kovács%'
order by nev desc;

select * from Termekek
where tipus = 1
order by megnevezes asc;

/*Csoportosító lekérdezések*/
select megnevezes, sum(mennyiseg) darab
from Termekek full outer join rendelesXtermekek on Termekek.termek_id = rendelesXtermekek.termek_id
group by megnevezes
having sum(mennyiseg)>3;

select Felhasznalok.nev, count(*) db
from Felhasznalok full outer join Futar on Felhasznalok.felhasznalo_id = Futar.felhasznalo_id
full outer join Rendelesek on Futar.futar_id = Rendelesek.futar_id
group by Felhasznalok.nev
having count(*) > 10;

select tipus, count(*)
from Termekek
where megnevezes like '%a%'
group by tipus
having count(*) > 4;

/*Többtáblás lekérdezések*/
select Termekek.megnevezes 
from Termekek inner join rendelesXtermekek on Termekek.termek_id = rendelesXtermekek.termek_id
inner join Rendelesek on rendelesXtermekek.rendeles_id = rendelesek.rendeles_id 
inner join Felhasznalok on rendelesek.felhasznalo_id = Felhasznalok.felhasznalo_id
where Felhasznalok.nev like 'Juhász Tamás';

select nev
from Felhasznalok left outer join Ertekelesek on Felhasznalok.felhasznalo_id = Ertekelesek.felhasznalo_id
where Ertekelesek.felhasznalo_id is null;

select Felhasznalok.nev, Ertekelesek.pontszam, Ertekelesek.szoveges_velemeny
from Felhasznalok inner join Ertekelesek on Felhasznalok.felhasznalo_id = Ertekelesek.felhasznalo_id;

/*Allekérdezések*/
select nev
from Felhasznalok
where felhasznalo_id in
(select felhasznalo_id from Felhasznalok
minus
select felhasznalo_id from Rendelesek);

select megnevezes from Termekek
where length(megnevezes) = any
(select max(length(megnevezes)) from Termekek);

select distinct felhasznalo_id, nev, mennyiseg
from Felhasznalok inner join Rendelesek using(felhasznalo_id) inner join rendelesXtermekek using(rendeles_id)
where rendelesxtermekek.mennyiseg = all
(select max(mennyiseg)
from rendelesxtermekek);

select avg(pontszam)
from Ertekelesek
where exists (select szoveges_velemeny from Ertekelesek where szoveges_velemeny is not null);

/*Halmaz operátorok*/
select felhasznalo_id from Rendelesek
union
select felhasznalo_id from Ertekelesek;

select felhasznalo_id from Felhasznalok
intersect
select felhasznalo_id from Rendelesek;

/*Függvények*/
select SUBSTR(email, INSTR(email, '@', 1, 1)+1) as LEVELEZŐSZOLGÁLTATÁS, COUNT(*) AS DARAB
FROM Felhasznalok
group by SUBSTR(email, INSTR(email, '@', 1, 1)+1);

select round(avg(fizetes)) Átlagfizetés
from Futar;

select upper(nev)
from Felhasznalok;

/*Nézetek*/
create or replace view ItalRendelok as
select Felhasznalok.felhasznalo_id, Felhasznalok.nev from Felhasznalok 
inner join Rendelesek on Felhasznalok.felhasznalo_id = Rendelesek.felhasznalo_id
inner join rendelesXtermekek on Rendelesek.rendeles_id = rendelesXtermekek.rendeles_id 
inner join Termekek on rendelesXtermekek.termek_id = Termekek.termek_id
where Termekek.tipus = 1;

select * from ItalRendelok;

create or replace view LegalacsonyabbFizetes as
select Felhasznalok.nev, Futar.futar_id, Futar.fizetes
from Felhasznalok 
inner join Futar on Felhasznalok.felhasznalo_id = Futar.felhasznalo_id
where Futar.fizetes = (select min(fizetes) from Futar);

select * from LegalacsonyabbFizetes;

create or replace view NemRendelt as
select Felhasznalok.felhasznalo_id, Felhasznalok.nev
from Felhasznalok
where felhasznalo_id in
(select felhasznalo_id from Felhasznalok
minus
select felhasznalo_id from Rendelesek);

select * from NemRendelt;

/*DML utasítások*/
alter table Felhasznalok
add kedvezmeny number(3);

/*Itt lesz egy mentés*/
savepoint mentes1;

update Felhasznalok
set kedvezmeny = 5
where felhasznalo_id in (select felhasznalo_id from ItalRendelok);

select * from Felhasznalok;

update Futar
set fizetes = fizetes * 1.02
where futar_id in (select futar_id from LegalacsonyabbFizetes);

select * from Futar;

update Felhasznalok
set nev = 'Kiss Laura'
where felhasznalo_id = 15;

select * from Felhasznalok;

delete from Ertekelesek
where szoveges_velemeny is null;

select * from Ertekelesek;


delete from Felhasznalok
where felhasznalo_id in (select felhasznalo_id from NemRendelt);

select * from Felhasznalok;

delete from Termekek
where tipus = 1;

select * from Termekek;

/*Visszatérés mentési pontra*/
rollback to mentes1;

/*Jogosultság-kezelés*/
create role Ugyintezo;
grant 
select any table,
create any table,
create procedure,
insert any table,
update any table,
delete any table,
create session
to Ugyintezo;

create user intezo
identified by intezo
quota unlimited on system;

grant Ugyintezo to intezo;

create role Rendszergazda;
grant
select any table,
create any table,
create procedure,
delete any table,
create session
to Rendszergazda;

create user rgazda
identified by rgazda
quota unlimited on system;

grant Rendszergazda to rgazda;

/*PL/SQL triggerek*/

set serveroutput on;

create or replace trigger Naplozo1
before delete or insert on Felhasznalok
for each row
begin
if inserting
then
dbms_output.put_line('Új: ' || :new.nev);
elsif deleting
then
dbms_output.put_line('Töröl: ' || :old.nev);
end if;
end;

create or replace trigger Naplozo2
before delete or insert on Termekek
for each row
begin
if inserting
then
dbms_output.put_line('Új: ' || :new.megnevezes);
elsif deleting
then
dbms_output.put_line('Töröl: ' || :old.megnevezes);
end if;
end;

create or replace trigger ujfelhasznalo
after insert on Felhasznalok
begin
dbms_output.put_line('Új felhasználó regisztrált!');
end;

/*Triggerek tesztelése*/
delete from Felhasznalok
where felhasznalo_id = 13;

insert into Termekek
values(90,1,'sör',300);

insert into Felhasznalok
values(80,'Teszt Elek', 'Teszt utca','123456789','tesztelek@gmail.com',null);

/*DDL-utasítáok*/
alter table Felhasznalok
add szuletesiido date;
select * from Felhasznalok;

alter table Felhasznalok
drop column szuletesiido;
select * from Felhasznalok;

alter table Termekek
drop constraint t_ck;
alter table Termekek
add constraint t_ck check(tipus between 0 and 2);

alter table Termekek
disable constraint t_ck;

alter table Felhasznalok
rename column nev to teljesnev;
select * from Felhasznalok;

rename Termekek to Products;