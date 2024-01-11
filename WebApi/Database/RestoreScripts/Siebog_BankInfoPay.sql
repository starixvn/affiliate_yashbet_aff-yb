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
-- Table structure for table `BankInfoPay`
--

DROP TABLE IF EXISTS `BankInfoPay`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `BankInfoPay` (
  `PayId` int(11) NOT NULL,
  `SysId` int(11) NOT NULL,
  `BankName` varchar(100) DEFAULT NULL,
  `BankCode` varchar(10) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  PRIMARY KEY (`PayId`,`SysId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `BankInfoPay`
--

LOCK TABLES `BankInfoPay` WRITE;
/*!40000 ALTER TABLE `BankInfoPay` DISABLE KEYS */;
INSERT INTO `BankInfoPay` VALUES (1,1,'Asia Commercial Bank','ACB'),(1,2,'Bank for Investment and Development of Vietnam','BIDV'),(1,3,'DongA Bank','DAB'),(1,4,'Sacombank','SACOM'),(1,5,'Techcombank','TCB'),(1,6,'Vietcombank','VCB'),(1,7,'Vietinbank','VTB'),(1,8,'Karsikorn Bank (K-Bank)','KKR'),(1,9,'Krung Thai Bank','KTB'),(1,10,'Siam Commercial Bank','SCB'),(1,11,'Bangkok Bank','BBL'),(1,12,'Bank of Ayudhya (Krungsri)','BOA'),(1,13,'TMB Bank Public Company Limited','TMB'),(1,15,'CIMB Thai','CIMBT'),(1,17,'Government Savings Bank','GSB'),(1,19,'Kiatnakin Bank','KNK'),(1,24,'Eximbank Vietnam','EXIM'),(1,26,'Bank Central Asia','BCA'),(1,27,'Mandiri Bank','MDR'),(1,28,'Bank Negara Indonesia','BNI'),(1,29,'Bank Rakyat Indonesia','BRI'),(1,30,'CIMB Niaga','CIMBN'),(1,31,'Maybank Berhad','MBB'),(1,32,'Public Bank BerhadCIMB Bank Berhad','PBB'),(1,33,'CIMB Bank Berhad','CIMB'),(1,34,'RHB Banking Group','RHB'),(1,35,'Hong Leong Bank Berhad','HLB'),(1,37,'AmBank Group','AMB'),(1,38,'Bank Islam Malaysia','BIMB'),(2,1,'Asia Commercial Bank','ACB.VN'),(2,2,'BIDV Bank','BIDV.VN'),(2,3,'DongA Bank','DAB.VN'),(2,4,'Sacombank','SCM.VN'),(2,5,'Techcombank','TCB.VN'),(2,6,'Vietcombank','VCB.VN'),(2,7,'Vietinbank','VTB.VN'),(2,8,'Kasikorn Bank','KBANK.TH'),(2,9,'Krung Thai Bank','KTB.TH'),(2,10,'Siam Commercial Bank','SCB.TH'),(2,11,'Bangkok bank','BBL.TH'),(2,13,'Thai Military Bank','TMB.TH'),(2,17,'Government Savings Bank','GSB.TH'),(2,24,'Eximbank','EXIM.VN'),(2,26,'Bank Central Asia','BCA.ID'),(2,27,'Bank Mandiri (VA)','MDR.VA.ID'),(2,28,'Bank Negara Indonesia (VA)','BNI.VA.ID'),(2,29,'Bank Rakyat (VA)','BRI.VA.ID'),(2,31,'Maybank Berhad','MBB.MY'),(2,32,'Public Bank','PBB.MY'),(2,33,'CIMB Bank','CIMB.MY'),(2,34,'RHB','RHB.MY'),(2,35,'Hong Leong Bank','HLB.MY'),(2,44,'Agribank','VBARD.VN'),(2,45,'Krungsri bank','BAY.TH'),(2,46,'Maybank Indonesia (VA)','MBBI.VA.ID'),(3,1,'Asia Commercial Bank','ACB'),(3,2,'BIVD','BIDV'),(3,3,'DongA Bank','DAB'),(3,4,'Sacom Bank','SCB'),(3,5,'Techcom Bank','TCB'),(3,6,'Vietcom Bank','VCB'),(3,7,'Vietin Bank','VTB'),(3,24,'Eximbank Vietnam','EXIM'),(3,39,'Vietnam International Commercial Joint Stock Bank','VIB'),(3,40,'Saigon – Hanoi Commercial Joint Stock Bank','SHB'),(3,41,'Tien Phong Commercial Joint Stock Bank','TPB'),(3,42,'Vietnam Bank for Agriculture and Rural Development','AGB'),(3,43,'Saigon Commercial BAnk','SCB');
/*!40000 ALTER TABLE `BankInfoPay` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-02-19 16:06:18
