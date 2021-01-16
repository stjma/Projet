create database hopital
go
/*
*******************
* Section Patient *
*******************
*/
use hopital
go
create table [Patient] (
	Code_Patient int constraint pk_Patient_CodePatient primary key not null,
	Nom_Patient varchar(50) null,
	Prenom_Patient varchar(50) null,
	NAS_Patient varchar(50) null,
	DateNaissance_Patient datetime null,
	Allergie_Patient varchar(50) null,
	Maladie_Original_Patient varchar(50) null,
	Date_Transplant_Patient datetime null,
	Coter_Transplant_Patient varchar(50) null,
	ABV_Patient int null,
	Anergy_Patient varchar(50) null,
	PPD_Patient Bit null,
	HbsAg_Patient Bit null,
	CMV_Patient Bit null,
	Transfusion_Patient varchar(50) null,
	VV_ByPass_Patient bit null,
	Antifib_Lytics_Patient bit null,
);

create table [Donneur](
	Code_Do int constraint pk_Donneur_CodeDonneur primary key not null,
	Vivant_Do bit null,
	ADN_Do varchar(50) null,
	Allergie_Do varchar(50) null,
	Raison_Deces_Do varchar(50) null,
	Virus varchar(50) null,
	Groupe_Sanguin_Do varchar(50) null,
	Date_Transplant_Do Datetime null,
	Code_Patient int constraint fk_Donneur_CodePatient foreign key (Code_Patient) references Patient(Code_Patient) on delete cascade  not null
);

create table [Transplantation] (
	Code_Transplant int constraint pk_Transplantation_CodeTransplant primary key not null,
	Nom_Medecin varchar(50) null,
	Code_Patient int constraint fk_Transplantation_CodePatient foreign key  references Patient(Code_Patient) on delete cascade  not null,
	Code_Donneur int constraint fk_Transplantation_CodeDnneur foreign key references Donneur(Code_Do) on delete no action not null
);
create table [Categorie] (
	Code_Categorie int constraint pk_Categorie_CodeCategorie  primary key not null identity (1,1),
	Nom_Categorie varchar(50) null
);
create table [Tests](
	Code_Test int constraint pk_Test_CodeTest primary key (Code_Test) not null,
	Nom_test varchar(50) null,
	Code_Categorie int constraint fk_Test_CodeCategorie foreign key references Categorie(Code_Categorie) on delete cascade  not null,
);
create table [Tests Patient](
	Code_Patient_TP int  not null,
	Code_Test_TP int not null,
	Resultat varchar(50) null,
	Date_test datetime null,
	Code_Patient int constraint fk_TestPatient_CodePatient foreign key (Code_Patient) references Patient(Code_Patient) on delete cascade  not null,
	Code_Test int constraint fk_TestPatient_CodeTest foreign key  references Tests(Code_Test) on delete cascade not null,
	constraint pk_TestPatient_CodePatient primary key (Code_Patient_TP,Code_Test_TP),
);

/*
****************
* Section Role *
****************
1 - Administrateur
2 - Super-User
3 - Médecin
4 - Utilisateur de base
*/
create table [Role_User](
	Code_Role int constraint pk_RoleUser_CodeRole primary key (Code_Role) not null,
	Nom_Role varchar(50) null,
);
create table [Utilisateur] (
	Code_U int constraint pk_Utilisateur_CodeU primary key (Code_U) not null,
	Nom_U varchar(50) null,
	Mdp_Y varchar(200) null,
	Nom_Utilisateur_U varchar(50) null,
	Prenom_U varchar(50) null,
	Code_Role int default 4 constraint fk_Utilisateur_CodeRole foreign key references Role_User(Code_Role) on delete cascade not null ,/*On met une valeur par défaut a chaque role. Dans ce cas-si, la role que l'on met par défaut est Utilisateur de base*/
	
);

