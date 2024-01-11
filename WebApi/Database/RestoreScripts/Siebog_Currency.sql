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
-- Table structure for table `Currency`
--

DROP TABLE IF EXISTS `Currency`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Currency` (
  `CurrencyId` tinyint(4) NOT NULL COMMENT 'Currency ID',
  `IsoCode` char(3) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Note` varchar(45) NOT NULL,
  PRIMARY KEY (`CurrencyId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='Currency';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Currency`
--

LOCK TABLES `Currency` WRITE;
/*!40000 ALTER TABLE `Currency` DISABLE KEYS */;
INSERT INTO `Currency` VALUES (2,'MYR','Malaysian Ringgit'),(3,'USD','US Dollar'),(4,'THB','Thai Baht'),(5,'HKD','Hong Kong Dollar'),(6,'EUR','Euro'),(9,'AUD','Australian Dollar'),(12,'GBP','British Pound'),(13,'RMB','Chinese Renminbi'),(15,'IDR','Indonesia Rupiah'),(32,'JAP','Japan Yen'),(41,'CHF','Swiss Franc'),(45,'WON','Korean Won'),(46,'BND','Brunei Dollar'),(49,'MXN','Mexican Peso'),(50,'CAN','Canadian Dollar'),(51,'VND','Vietnamese Dong'),(52,'DKK','Danish Krone'),(53,'SEK','Swedish Krona'),(54,'NOK','Norwegian Krone'),(55,'RUB','Russian Ruble'),(56,'PLN','Polish Zloty'),(57,'CZK','Czech Koruny'),(58,'RON','Romanian Leu'),(61,'INR','Indian Rupee'),(71,'KHR','Cambodian Riel'),(73,'LIR','Turkish Lira');
/*!40000 ALTER TABLE `Currency` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-02-19 16:06:30
