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
-- Table structure for table `PayList`
--

DROP TABLE IF EXISTS `PayList`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `PayList` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PayName` varchar(45) NOT NULL,
  `Setting` json DEFAULT NULL,
  `Status` tinyint(4) NOT NULL DEFAULT '1',
  `Sort` varchar(45) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `PayList`
--

LOCK TABLES `PayList` WRITE;
/*!40000 ALTER TABLE `PayList` DISABLE KEYS */;
INSERT INTO `PayList` VALUES (1,'Help2Pay','{\"key\": \"RVYPiMvk6Qphi57CBJZj\", \"host\": \"http://api.besthappylife.biz/MerchantTransfer\", \"PayOut\": \"http://app.besthappylife.biz/MerchantPayout/M0620\", \"PayName\": \"Help2Pay\", \"merchant_no\": \"M0620\"}',1,'0'),(2,'Eeziepay','{\"key\": \"BAa0ceWVYPVmmfrQSaw8Cg5eSpEfF9Iv\", \"host\": \"https://www.yummyshop718.com/fundtransfer.php\", \"PayOut\": \"https://www.yummyshop718.com/withdrawal.php\", \"PayName\": \"Eeziepay\", \"merchant_no\": \"NOVA0001\"}',1,'1');
/*!40000 ALTER TABLE `PayList` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-02-19 16:06:12
