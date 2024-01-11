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
-- Table structure for table `TpList`
--

DROP TABLE IF EXISTS `TpList`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `TpList` (
  `Id` int(11) NOT NULL,
  `TpName` varchar(45) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Setting` json DEFAULT NULL,
  `Status` tinyint(4) NOT NULL DEFAULT '1' COMMENT '0: Disabled, 1:Enabled, 2:UM',
  `Sort` smallint(6) NOT NULL DEFAULT '0',
  `TicketKey` bigint(20) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `TpList`
--

LOCK TABLES `TpList` WRITE;
/*!40000 ALTER TABLE `TpList` DISABLE KEYS */;
INSERT INTO `TpList` VALUES (1,'Saba','{\"ApiUrl\": \"http://tsa.n88dev.com/api\", \"VendorId\": \"aakx8ncolf\", \"MobileUrl\": \"http://smartsbtest.n88dev.com\", \"NativeUrl\": \"http://smartsbtest.n88dev.com\", \"DesktopUrl\": \"http://sbtest.n88dev.com/deposit_processlogin.aspx\", \"MobileBfUrl\": \"http://smartsbtest.n88dev.com/depositlogin/bfindex\", \"DesktopBfUrl\": \"http://sbtest.n88dev.com/Newindex\"}',1,0,165124),(2,'SA','{\"ApiUrl\": \" http://sai-api.sa-apisvr.com/api/api.aspx\", \"MD5Key\": \"GgaIMaiNNtg\", \"EncrypKey\": \"g9G16nTs\", \"MobileUrl\": \"https://www.sai.slgaming.net/app.aspx\", \"NativeUrl\": \"https://www.sai.slgaming.net/app.aspx\", \"SecretKey\": \"aakx8ncolf\", \"DesktopUrl\": \"https://www.sai.slgaming.net/app.aspx\", \"MobileBfUrl\": \"http://smartsbtest.n88dev.com/depositlogin/bfindex\", \"DesktopBfUrl\": \"https://www.sai.slgaming.net/app.aspx\"}',1,0,0);
/*!40000 ALTER TABLE `TpList` ENABLE KEYS */;
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
