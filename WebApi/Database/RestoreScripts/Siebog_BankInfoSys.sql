CREATE DATABASE  IF NOT EXISTS `Siebog` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */;
USE `Siebog`;
-- MySQL dump 10.13  Distrib 8.0.19, for Win64 (x86_64)
--
-- Host: 192.168.2.64    Database: Siebog
-- ------------------------------------------------------
-- Server version	8.0.13

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `BankInfoSys`
--

DROP TABLE IF EXISTS `BankInfoSys`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `BankInfoSys` (
  `Id` int(11) NOT NULL,
  `CurrencyId` int(11) NOT NULL,
  `BankCode` varchar(10) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `BankName` varchar(100) DEFAULT NULL,
  `BankLogo` varchar(45) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `BankSite` varchar(255) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `Status` tinyint(4) NOT NULL DEFAULT '1' COMMENT '0: Closed\n1: Enabled',
  `Sort` smallint(6) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `BankInfoSysIdx1` (`CurrencyId`,`BankCode`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `BankInfoSys`
--

LOCK TABLES `BankInfoSys` WRITE;
/*!40000 ALTER TABLE `BankInfoSys` DISABLE KEYS */;
INSERT INTO `BankInfoSys` VALUES (1,51,'ACB','Asia Commercial Bank','ACB.png','https://online.acb.com.vn/acbib',1,0),(2,51,'BIDV','BIDV','BIDV.png',NULL,1,0),(3,51,'DAB','DongA Bank','DAB.png','https://ebanking.dongabank.com.vn/khcn/',1,0),(4,51,'SACOM','Sacom Bank','SAC.png',NULL,1,0),(5,51,'TCB','Techcom Bank','TCB.png','https://ib.techcombank.com.vn/servlet/BrowserServlet',1,0),(6,51,'VCB','Vietcom Bank','VCB.png','https://www.vietcombank.com.vn/IBanking2015',1,0),(7,51,'VTB','Vietin Bank','VTB.png','https://ebanking.vietinbank.vn/rcas/portal/web/retail/bflogin',1,0),(8,4,'KBANK','Kasikorn Bank','KKR.png','https://online.kasikornbankgroup.com/K-Online/login.jsp',1,0),(9,4,'KTB','Krung Thai Bank','KTB.png','https://www.ktb.co.th/th/personal',1,0),(10,4,'SCB','Siam Commercial Bank','SCB.png','https://www.scbeasy.com/v1.4/site/presignon/en/srv/srv.asp',1,0),(11,4,'BBL','Bangkok Bank','BBL.png','https://ibanking.bangkokbank.com/SignOn.aspx',1,0),(12,4,'BOA','Bank of Ayudhya','BOA.png','https://www.krungsri.com/bank/th/home.html',1,0),(13,4,'TMB','TMB Bank','TMB.png',NULL,1,0),(14,4,'TBACI','Bank for Agriculture and Agricultural Cooperatives',NULL,NULL,0,0),(15,4,'CIMBT','CIMB Thai','CIMBT.png',NULL,1,0),(16,4,'TGHBI','Government Housing Bank','GHB.png',NULL,1,0),(17,4,'GSB','Government Saving Bank','GSB.png',NULL,1,0),(18,4,'TILBI','Islamic Bank',NULL,NULL,0,0),(19,4,'KKR','Kiatnakin Bank','KNK.png',NULL,1,0),(20,4,'TLHBI','LH Bank',NULL,NULL,0,0),(21,4,'TSCTI','Standard Chartered Bank',NULL,NULL,0,0),(22,4,'TTNBI','Thanachart Bank',NULL,NULL,0,0),(23,4,'TUOTI','United Overseas Bank',NULL,NULL,0,0),(24,51,'EXIM','Eximbank Vietnam','EXIM.png',NULL,1,0),(26,15,'BCA','Bank Central Asia','BCA.png','https://www.klikbca.com/',1,0),(27,15,'MDR','Mandiri Bank','MDR.png','https://ib.bankmandiri.co.id/retail/Login.do?action=form&lang=in_ID',1,0),(28,15,'BNI','Bank Negara Indonesia','BNI.png','https://ibank.bni.co.id/',1,0),(29,15,'BRI','Bank Rakyat Indonesia','BRI.png','https://ib.bri.co.id/ib-bri',1,0),(30,15,'CIMBN','CIMB Niaga','CIMBN.png','https://www.cimbclicks.co.id/ib-cimbniaga/Login.html',1,0),(31,2,'MBB','Maybank Berhad','MBB.png','https://www.maybank2u.com.my/home/m2u/common/login.do',1,0),(32,2,'PBB','Public Bank Berhad','PBB.png','https://www2.pbebank.com/myIBK/apppbb/servlet/',1,0),(33,2,'CIMB','CIMB Bank Berhad','CIMB.png','https://www.cimbclicks.com.my/clicks/#/',1,0),(34,2,'RHB','RHB Banking Group','RHB.png','https://logon.rhb.com.my/lgn_default.aspx?usercontrol=Login/lgn_first_time_enrol_card_sel',1,0),(35,2,'HLB','Hong Leong Bank Berhad','HLB.png','https://s.hongleongconnect.my/rib/app/fo/login',1,0),(37,2,'AMB','AmBank Group','AMB.png',NULL,1,0),(38,2,'BIMB','Bank Islam Malaysia','BIMB.png',NULL,1,0),(39,51,'VIB','Vietnam International Commercial Joint Stock Bank','VIB.png',NULL,1,0),(40,51,'SHB','Saigon – Hanoi Commercial Joint Stock Bank','SHB.png',NULL,1,0),(41,51,'TPB','Tien Phong Commercial Joint Stock Bank','TPB.png',NULL,1,0),(42,51,'AGB','Vietnam Bank for Agriculture and Rural Development','AGB.png',NULL,1,0),(43,51,'SCB','Saigon Commercial BAnk','SCBV.png',NULL,1,0),(44,51,'VBARD','Agri Bank',NULL,NULL,1,0),(45,4,'BAY','Krungsri Bank',NULL,NULL,1,0),(46,15,'MBBI','Maybank Indonesia',NULL,NULL,1,0);
/*!40000 ALTER TABLE `BankInfoSys` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-02-19 16:06:05
