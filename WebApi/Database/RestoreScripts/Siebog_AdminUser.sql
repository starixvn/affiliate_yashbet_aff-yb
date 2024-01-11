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
-- Table structure for table `AdminUser`
--

DROP TABLE IF EXISTS `AdminUser`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `AdminUser` (
  `UserId` int(11) NOT NULL AUTO_INCREMENT,
  `SiteId` int(11) NOT NULL,
  `UserName` varchar(30) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Userpwd` varchar(50) CHARACTER SET ascii COLLATE ascii_bin NOT NULL,
  `Role` json DEFAULT NULL,
  `IsSuper` tinyint(1) NOT NULL DEFAULT '0',
  `Status` tinyint(4) NOT NULL DEFAULT '1' COMMENT '1: Running, 2: Suspend, 3: Closed',
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`UserId`),
  UNIQUE KEY `AdminUserIdx1` (`SiteId`,`UserName`)
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `AdminUser`
--

LOCK TABLES `AdminUser` WRITE;
/*!40000 ALTER TABLE `AdminUser` DISABLE KEYS */;
INSERT INTO `AdminUser` VALUES (1,1,'Admin','5416d7cd6ef195a0f7622a9c56b55e84','[\"Admin\", \"CSD\", \"Finance\"]',1,1,'2019-12-05 09:17:38'),(2,1,'test','e10adc3949ba59abbe56e057f20f883e','[\"Admin\", \"CSD\"]',0,1,'2019-12-09 08:34:18'),(3,1,'eddie','e10adc3949ba59abbe56e057f20f883e','[\"CSD\", \"Finance\"]',1,1,'2019-12-09 08:35:53'),(10,1,'scott0108','6562c5c1f33db6e05a082a88cddab5ea','[\"Admin\"]',0,1,'2019-12-09 10:11:14'),(13,1,'1234','1234','[\"Admin\", \"CSD\", \"Finance\"]',0,1,'2020-01-17 06:23:52'),(14,1,'11111','79d886010186eb60e3611cd4a5d0bcae','[\"Admin\", \"CSD\", \"Finance\"]',0,1,'2020-01-17 06:36:44'),(17,1,'resTest','c37bd5b91038197293ec8270a59f563e','[\"CSD\"]',0,1,'2020-01-20 08:14:23'),(18,1,'nvcalen','e10adc3949ba59abbe56e057f20f883e','[\"CSD\", \"Finance\"]',0,1,'2020-01-21 05:45:14'),(19,1,'CNVEllis','e10adc3949ba59abbe56e057f20f883e','[\"Admin\", \"CSD\", \"Finance\"]',0,0,'2020-01-21 10:24:23'),(20,1,'NVCEllis','e10adc3949ba59abbe56e057f20f883e','[\"Admin\", \"CSD\", \"Finance\"]',0,1,'2020-01-21 10:25:06'),(21,1,'byrontest','33374c88e3a7570c211c203b4795fc24','[\"CSD\", \"Finance\"]',0,1,'2020-01-26 05:39:26'),(22,1,'','d41d8cd98f00b204e9800998ecf8427e','[]',0,1,'2020-01-27 08:56:29'),(23,1,'celinetest001','e10adc3949ba59abbe56e057f20f883e','[\"Admin\", \"CSD\", \"Finance\"]',0,1,'2020-02-11 10:35:48'),(24,1,'celinetest002','e10adc3949ba59abbe56e057f20f883e','[\"Admin\", \"CSD\", \"Finance\"]',0,1,'2020-02-11 10:39:13');
/*!40000 ALTER TABLE `AdminUser` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-02-19 16:06:07
