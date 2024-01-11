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
-- Table structure for table `ServerInfo`
--

DROP TABLE IF EXISTS `ServerInfo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ServerInfo` (
  `SiteId` int(11) NOT NULL COMMENT 'Site ID',
  `SiteName` varchar(50) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL COMMENT 'Site Name',
  `LangList` json DEFAULT NULL COMMENT 'Language List',
  `CurrencyList` json DEFAULT NULL COMMENT 'Currency List',
  `DefLang` varchar(4) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `DefCurrency` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`SiteId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='Server Info';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ServerInfo`
--

LOCK TABLES `ServerInfo` WRITE;
/*!40000 ALTER TABLE `ServerInfo` DISABLE KEYS */;
INSERT INTO `ServerInfo` VALUES (1,'Nova88','[\"en\", \"cs\", \"ch\", \"vn\", \"th\", \"id\"]','[2, 4, 13, 15, 51]','en',2);
/*!40000 ALTER TABLE `ServerInfo` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-02-19 16:06:01
