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
-- Table structure for table `AdjustmentInfo`
--

DROP TABLE IF EXISTS `AdjustmentInfo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `AdjustmentInfo` (
  `AdjustId` int(11) NOT NULL AUTO_INCREMENT,
  `TransId` varchar(30) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `AdjustType` tinyint(4) NOT NULL DEFAULT '1' COMMENT '1: Debit, 2: Credit, 3: Bonus',
  `CustId` int(11) NOT NULL,
  `UserName` varchar(30) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `CurrencyId` tinyint(4) NOT NULL DEFAULT '0',
  `SiteId` int(11) NOT NULL,
  `AdjustIdInfo` json DEFAULT NULL,
  `Amount` decimal(18,4) NOT NULL DEFAULT '0.0000',
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `FirstDeposit` bit(1) NOT NULL DEFAULT b'0',
  `RefId` varchar(100) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `Memo` varchar(100) DEFAULT NULL,
  `Comment` varchar(500) DEFAULT NULL,
  `Status` tinyint(4) NOT NULL DEFAULT '1' COMMENT '1. Pending, 2, Approved 3, Rejected',
  PRIMARY KEY (`AdjustId`)
) ENGINE=InnoDB AUTO_INCREMENT=445 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AdjustmentInfo`
--

LOCK TABLES `AdjustmentInfo` WRITE;
/*!40000 ALTER TABLE `AdjustmentInfo` DISABLE KEYS */;
INSERT INTO `AdjustmentInfo` VALUES (373,'AD373',1,9,'qatest05',51,1,NULL,10000.0000,'2020-01-14 08:28:32',_binary '\0',NULL,NULL,NULL,1),(379,'AD379',1,9,'qatest05',51,1,NULL,1000.0000,'2020-01-14 08:38:57',_binary '\0',NULL,NULL,'Payment Adjustment: 1000',2),(380,'AD380',1,9,'qatest05',51,1,NULL,2000.0000,'2020-01-14 08:39:53',_binary '\0',NULL,NULL,'Payment Adjustment: 2000',2),(381,'AD381',1,9,'qatest05',51,1,NULL,1000.0000,'2020-01-14 08:41:26',_binary '\0',NULL,NULL,'Payment Adjustment: 1000',2),(382,'AD382',1,9,'qatest05',51,1,NULL,11.0000,'2020-01-14 10:01:01',_binary '\0',NULL,NULL,'Payment Adjustment: 11',2),(383,'AD383',1,9,'qatest05',51,1,NULL,11.0000,'2020-01-14 10:10:14',_binary '\0',NULL,NULL,'Payment Adjustment: 11',2),(384,'AD384',1,9,'qatest05',51,1,NULL,11.0000,'2020-01-14 10:10:36',_binary '\0',NULL,NULL,'Payment Adjustment: 11',2),(385,'AD385',1,9,'qatest05',51,1,NULL,1000.0000,'2020-01-14 10:17:13',_binary '\0',NULL,NULL,'Payment Adjustment: 1000',2),(386,'AD386',2,9,'qatest05',51,1,NULL,1000.0000,'2020-01-14 10:17:27',_binary '\0',NULL,NULL,'Payment Adjustment: -1000',2),(387,'AD387',2,9,'qatest05',51,1,NULL,1000.0000,'2020-01-14 10:18:38',_binary '\0',NULL,NULL,'Payment Adjustment: -1000',2),(388,'AD388',2,9,'qatest05',51,1,NULL,1000.0000,'2020-01-14 10:20:19',_binary '\0',NULL,NULL,'Payment Adjustment: -1000',2),(389,'AD389',1,9,'qatest05',51,1,NULL,1000.0000,'2020-01-14 10:21:48',_binary '\0',NULL,NULL,'Payment Adjustment: 1000',2),(390,'AD390',2,9,'qatest05',51,1,NULL,1000.0000,'2020-01-14 10:21:54',_binary '\0',NULL,NULL,'Payment Adjustment: -1000',2),(391,'AD391',2,9,'qatest05',51,1,NULL,1000.0000,'2020-01-14 10:45:05',_binary '\0',NULL,NULL,'Payment Adjustment: -1000',2),(392,'AD392',1,9,'qatest05',51,1,NULL,1000.0000,'2020-01-14 10:45:15',_binary '\0',NULL,NULL,'Payment Adjustment: 1000',2),(393,'AD393',2,7,'qatest01',51,1,NULL,1000.0000,'2020-01-14 10:46:45',_binary '\0',NULL,NULL,'Payment Adjustment: -1000',2),(394,'AD394',2,7,'qatest01',51,1,NULL,1990.0000,'2020-01-14 10:47:05',_binary '\0',NULL,NULL,'Payment Adjustment: -1990',2),(395,'AD395',1,7,'qatest01',51,1,NULL,1000.0000,'2020-01-14 10:47:15',_binary '\0',NULL,NULL,'Payment Adjustment: 1000',2),(396,'AD396',1,65,'Dongralph',51,1,NULL,1000.0000,'2020-01-14 11:07:40',_binary '\0',NULL,NULL,'Payment Adjustment: 1000',2),(397,'AD397',1,65,'Dongralph',51,1,NULL,10.0000,'2020-01-14 11:08:06',_binary '\0',NULL,NULL,'Payment Adjustment: 10',2),(398,'AD398',1,65,'Dongralph',51,1,NULL,100.0000,'2020-01-14 11:08:29',_binary '\0',NULL,NULL,'Payment Adjustment: 100',2),(399,'AD399',1,65,'Dongralph',51,1,NULL,100.0000,'2020-01-14 11:09:06',_binary '\0',NULL,NULL,'Payment Adjustment: 100',2),(400,'AD400',1,65,'Dongralph',51,1,NULL,1000.0000,'2020-01-14 11:11:40',_binary '\0',NULL,NULL,'Payment Adjustment: 1000',2),(401,'AD401',1,65,'Dongralph',51,1,NULL,10000.0000,'2020-01-14 11:12:47',_binary '\0',NULL,NULL,'Payment Adjustment: 10000',2),(402,'AD402',1,65,'Dongralph',51,1,NULL,10000000.0000,'2020-01-14 11:13:38',_binary '\0',NULL,NULL,'Payment Adjustment: 10000000',2),(403,'AD403',2,9,'qatest05',51,1,NULL,1010.0000,'2020-01-14 11:16:24',_binary '\0',NULL,NULL,'Payment Adjustment: -1010',2),(404,'AD404',1,42,'ralph799',2,1,NULL,100000000.0000,'2020-01-14 12:24:16',_binary '\0',NULL,NULL,'Payment Adjustment: 100000000',2),(407,'AD407',1,40,'ckm5133',2,1,NULL,100.0000,'2020-01-15 07:14:36',_binary '\0',NULL,NULL,'Payment Adjustment: 100',2),(411,'AD411',1,40,'ckm5133',2,1,NULL,100.0000,'2020-01-16 07:24:22',_binary '\0',NULL,NULL,'Payment Adjustment: 100',2),(414,'AD414',1,7,'qatest01',51,1,NULL,1000.0000,'2020-01-16 07:39:55',_binary '\0',NULL,NULL,'Payment Adjustment: 1000',2),(420,'AD420',2,3,'scott0108',4,1,NULL,1000.0000,'2020-01-16 07:48:10',_binary '\0',NULL,NULL,'Payment Adjustment: -1000',2),(421,'AD421',2,7,'qatest01',51,1,NULL,1000.0000,'2020-01-16 07:49:56',_binary '\0',NULL,NULL,'Payment Adjustment: -1000',2),(422,'AD422',1,7,'qatest01',51,1,NULL,1000.0000,'2020-01-16 07:50:21',_binary '\0',NULL,NULL,'Payment Adjustment: 1000',2),(423,'AD423',1,7,'qatest01',51,1,NULL,1000.0000,'2020-01-16 07:50:40',_binary '\0',NULL,NULL,'Payment Adjustment: 1000',2),(424,'AD424',1,7,'qatest01',51,1,NULL,1000.0000,'2020-01-16 07:51:32',_binary '\0',NULL,NULL,'Payment Adjustment: 1000',2),(425,'AD425',2,7,'qatest01',51,1,NULL,1000.0000,'2020-01-16 07:52:41',_binary '\0',NULL,NULL,'Payment Adjustment: -1000',2),(426,'AD426',1,7,'qatest01',51,1,NULL,1000.0000,'2020-01-16 07:52:55',_binary '\0',NULL,NULL,'Payment Adjustment: 1000',2),(427,'AD427',1,49,'eddieidr',15,1,NULL,1000.0000,'2020-01-16 07:54:25',_binary '\0',NULL,NULL,'Payment Adjustment: 1000',2),(428,'AD428',2,49,'eddieidr',15,1,NULL,2000.0000,'2020-01-16 07:54:51',_binary '\0',NULL,NULL,'Payment Adjustment: -2000',2),(429,'AD429',1,3,'scott0108',4,1,NULL,6.0000,'2020-01-16 08:04:38',_binary '\0',NULL,NULL,'Payment Adjustment: 6',2),(430,'AD430',2,3,'scott0108',4,1,NULL,50.0000,'2020-01-16 08:04:45',_binary '\0',NULL,NULL,'Payment Adjustment: -50',2),(431,'AD431',2,40,'ckm5133',2,1,NULL,100.0000,'2020-01-16 08:06:55',_binary '\0',NULL,NULL,'Payment Adjustment: -100',2),(432,'AD432',1,40,'ckm5133',2,1,NULL,10.0000,'2020-01-16 08:07:13',_binary '\0',NULL,NULL,'Payment Adjustment: 10',2),(433,'AD433',1,80,'lalaland',15,1,NULL,20.0000,'2020-01-16 11:32:14',_binary '\0',NULL,NULL,'Payment Adjustment: 20',2),(434,'AD434',1,80,'lalaland',15,1,NULL,20.0000,'2020-01-16 11:32:17',_binary '\0',NULL,NULL,'Payment Adjustment: 20',2),(435,'AD435',1,80,'lalaland',15,1,NULL,300.0000,'2020-01-16 11:32:23',_binary '\0',NULL,NULL,'Payment Adjustment: 300',2),(436,'AD436',2,40,'ckm5133',2,1,NULL,1000.0000,'2020-01-16 12:56:44',_binary '\0',NULL,NULL,'Payment Adjustment: -1000',2),(437,'AD437',1,40,'ckm5133',2,1,NULL,1000.0000,'2020-01-16 12:56:53',_binary '\0',NULL,NULL,'Payment Adjustment: 1000',2),(438,'AD438',2,40,'ckm5133',2,1,NULL,100.0000,'2020-01-17 08:49:23',_binary '\0',NULL,NULL,'Payment Adjustment: -100',2),(439,'AD439',1,107,'stan3889',2,1,NULL,10000.0000,'2020-01-20 14:30:58',_binary '\0',NULL,NULL,'Payment Adjustment: 10000',2),(440,'AD440',1,7,'qatest01',51,1,NULL,100.0000,'2020-01-26 04:58:34',_binary '\0',NULL,NULL,'Payment Adjustment: 100',2),(441,'AD441',2,7,'qatest01',51,1,NULL,100.0000,'2020-01-26 04:58:50',_binary '\0',NULL,NULL,'Payment Adjustment: -100',2),(442,'AD442',1,7,'qatest01',51,1,NULL,100.0000,'2020-01-26 04:59:01',_binary '\0',NULL,NULL,'Payment Adjustment: 100',2),(443,'AD443',1,7,'qatest01',51,1,NULL,1000.0000,'2020-01-26 05:02:08',_binary '\0',NULL,NULL,'Payment Adjustment: 1000',2),(444,'AD444',2,7,'qatest01',51,1,NULL,1000.0000,'2020-01-26 05:02:23',_binary '\0',NULL,NULL,'Payment Adjustment: -1000',2);
/*!40000 ALTER TABLE `AdjustmentInfo` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-02-19 16:06:08
