create table team
(
 stamnummer int not null primary key,
 naam nvarchar(150) not null,
 bijnaam nvarchar(150) null
 );

create table speler
(
	id int not null primary key identity,
	naam nvarchar(150) not null,
	rugnummer int null,
	lengte int null,
	gewicht int null,
	teamid int null,
	constraint FK_speler_team foreign key (teamid) references team(stamnummer)
	);
create table transfer
(
  id int not null primary key identity,
  spelerid int not null,
  prijs int not null,
  oudteamid int null,
  nieuwteamid int null,
  constraint FK_transfer_speler Foreign key (spelerid) references speler(id),
  constraint FK_transfer_oudteam foreign key(oudteamid) references team(stamnummer),
  constraint FK_transfer_nieuwteam foreign key(nieuwteamid) references team(stamnummer)
  );