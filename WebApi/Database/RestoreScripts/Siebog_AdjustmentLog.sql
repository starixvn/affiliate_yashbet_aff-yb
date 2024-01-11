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
-- Table structure for table `AdjustmentLog`
--

DROP TABLE IF EXISTS `AdjustmentLog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `AdjustmentLog` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AdjustId` int(11) NOT NULL,
  `SiteId` int(11) NOT NULL,
  `Action` tinyint(4) NOT NULL,
  `Comment` varchar(500) DEFAULT NULL,
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Operator` varchar(30) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=291 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AdjustmentLog`
--

LOCK TABLES `AdjustmentLog` WRITE;
/*!40000 ALTER TABLE `AdjustmentLog` DISABLE KEYS */;
INSERT INTO `AdjustmentLog` VALUES (219,373,1,99,'Parameter \'RefId\' not found in the collection.','2020-01-14 08:28:33','Admin'),(220,374,1,99,'Unknown column \'AdjustmentInfo.DepositId\' in \'where clause\'','2020-01-14 08:33:08','Admin'),(221,375,1,99,'Unknown column \'AdjustmentInfo.DepositId\' in \'where clause\'','2020-01-14 08:33:55','Admin'),(222,376,1,99,'Unknown column \'AdjustmentInfo.DepositId\' in \'where clause\'','2020-01-14 08:34:29','Admin'),(223,377,1,99,'Statement aborted because lock(s) could not be acquired immediately and NOWAIT is set.','2020-01-14 08:34:38','Admin'),(224,378,1,99,'Statement aborted because lock(s) could not be acquired immediately and NOWAIT is set.','2020-01-14 08:34:44','Admin'),(225,379,1,2,'Payment Adjustment: 1000','2020-01-14 08:38:58','Admin'),(226,380,1,2,'Payment Adjustment: 2000','2020-01-14 08:39:53','Admin'),(227,381,1,2,'Payment Adjustment: 1000','2020-01-14 08:41:26','Admin'),(228,382,1,2,'Payment Adjustment: 11','2020-01-14 10:01:02','Admin'),(229,383,1,2,'Payment Adjustment: 11','2020-01-14 10:10:15','Admin'),(230,384,1,2,'Payment Adjustment: 11','2020-01-14 10:10:36','Admin'),(231,385,1,2,'Payment Adjustment: 1000','2020-01-14 10:17:13','Admin'),(232,386,1,2,'Payment Adjustment: -1000','2020-01-14 10:17:27','Admin'),(233,387,1,2,'Payment Adjustment: -1000','2020-01-14 10:18:38','Admin'),(234,388,1,2,'Payment Adjustment: -1000','2020-01-14 10:20:19','Admin'),(235,389,1,2,'Payment Adjustment: 1000','2020-01-14 10:21:48','Admin'),(236,390,1,2,'Payment Adjustment: -1000','2020-01-14 10:21:54','Admin'),(237,391,1,2,'Payment Adjustment: -1000','2020-01-14 10:45:05','Admin'),(238,392,1,2,'Payment Adjustment: 1000','2020-01-14 10:45:15','Admin'),(239,393,1,2,'Payment Adjustment: -1000','2020-01-14 10:46:45','Admin'),(240,394,1,2,'Payment Adjustment: -1990','2020-01-14 10:47:05','Admin'),(241,395,1,2,'Payment Adjustment: 1000','2020-01-14 10:47:15','Admin'),(242,396,1,2,'Payment Adjustment: 1000','2020-01-14 11:07:40','test'),(243,397,1,2,'Payment Adjustment: 10','2020-01-14 11:08:06','test'),(244,398,1,2,'Payment Adjustment: 100','2020-01-14 11:08:29','test'),(245,399,1,2,'Payment Adjustment: 100','2020-01-14 11:09:06','test'),(246,400,1,2,'Payment Adjustment: 1000','2020-01-14 11:11:40','test'),(247,401,1,2,'Payment Adjustment: 10000','2020-01-14 11:12:47','test'),(248,402,1,2,'Payment Adjustment: 10000000','2020-01-14 11:13:38','test'),(249,403,1,2,'Payment Adjustment: -1010','2020-01-14 11:16:24','Admin'),(250,404,1,2,'Payment Adjustment: 100000000','2020-01-14 12:24:16','test'),(251,405,1,2,'Payment Adjustment: -1000000','2020-01-14 12:24:46','test'),(252,406,1,2,'Payment Adjustment: -1000000','2020-01-14 12:24:50','test'),(253,407,1,2,'Payment Adjustment: 100','2020-01-15 07:14:36','test'),(254,408,1,2,'Payment Adjustment: -100','2020-01-15 07:15:10','test'),(255,409,1,2,'Payment Adjustment: -100','2020-01-15 07:15:22','test'),(256,410,1,2,'Payment Adjustment: -100','2020-01-15 07:15:28','test'),(257,411,1,2,'Payment Adjustment: 100','2020-01-16 07:24:23','test'),(258,412,1,2,'Payment Adjustment: -1','2020-01-16 07:25:08','test'),(259,413,1,2,'Payment Adjustment: -1000','2020-01-16 07:39:50','Admin'),(260,414,1,2,'Payment Adjustment: 1000','2020-01-16 07:39:55','Admin'),(261,415,1,2,'Payment Adjustment: -1000','2020-01-16 07:40:39','Admin'),(262,416,1,2,'Payment Adjustment: -1000','2020-01-16 07:40:50','Admin'),(263,417,1,2,'Payment Adjustment: -100','2020-01-16 07:41:24','Admin'),(264,418,1,2,'Payment Adjustment: -100','2020-01-16 07:43:08','Admin'),(265,419,1,2,'Payment Adjustment: -100','2020-01-16 07:44:22','Admin'),(266,420,1,2,'Payment Adjustment: -1000','2020-01-16 07:48:10','Admin'),(267,421,1,2,'Payment Adjustment: -1000','2020-01-16 07:49:56','Admin'),(268,422,1,2,'Payment Adjustment: 1000','2020-01-16 07:50:21','Admin'),(269,423,1,2,'Payment Adjustment: 1000','2020-01-16 07:50:40','Admin'),(270,424,1,2,'Payment Adjustment: 1000','2020-01-16 07:51:32','Admin'),(271,425,1,2,'Payment Adjustment: -1000','2020-01-16 07:52:42','Admin'),(272,426,1,2,'Payment Adjustment: 1000','2020-01-16 07:52:55','Admin'),(273,427,1,2,'Payment Adjustment: 1000','2020-01-16 07:54:25','Admin'),(274,428,1,2,'Payment Adjustment: -2000','2020-01-16 07:54:51','Admin'),(275,429,1,2,'Payment Adjustment: 6','2020-01-16 08:04:38','Admin'),(276,430,1,2,'Payment Adjustment: -50','2020-01-16 08:04:45','Admin'),(277,431,1,2,'Payment Adjustment: -100','2020-01-16 08:06:55','test'),(278,432,1,2,'Payment Adjustment: 10','2020-01-16 08:07:13','test'),(279,433,1,2,'Payment Adjustment: 20','2020-01-16 11:32:14','test'),(280,434,1,2,'Payment Adjustment: 20','2020-01-16 11:32:17','test'),(281,435,1,2,'Payment Adjustment: 300','2020-01-16 11:32:23','test'),(282,436,1,2,'Payment Adjustment: -1000','2020-01-16 12:56:44','test'),(283,437,1,2,'Payment Adjustment: 1000','2020-01-16 12:56:53','test'),(284,438,1,2,'Payment Adjustment: -100','2020-01-17 08:49:23','test'),(285,439,1,2,'Payment Adjustment: 10000','2020-01-20 14:30:58','test'),(286,440,1,2,'Payment Adjustment: 100','2020-01-26 04:58:36','Admin'),(287,441,1,2,'Payment Adjustment: -100','2020-01-26 04:58:52','Admin'),(288,442,1,2,'Payment Adjustment: 100','2020-01-26 04:59:02','Admin'),(289,443,1,2,'Payment Adjustment: 1000','2020-01-26 05:02:13','Admin'),(290,444,1,2,'Payment Adjustment: -1000','2020-01-26 05:02:25','Admin');
/*!40000 ALTER TABLE `AdjustmentLog` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-02-19 16:06:33
