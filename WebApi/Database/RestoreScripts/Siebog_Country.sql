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
-- Table structure for table `Country`
--

DROP TABLE IF EXISTS `Country`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Country` (
  `Code` varchar(3) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `CountryName` varchar(45) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Sort` tinyint(4) NOT NULL DEFAULT '1',
  PRIMARY KEY (`Code`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Country`
--

LOCK TABLES `Country` WRITE;
/*!40000 ALTER TABLE `Country` DISABLE KEYS */;
INSERT INTO `Country` VALUES ('AF','Afghanistan',1),('AI','Anguilla',1),('AO','Angola',1),('AR','Argentina',1),('AW','Aruba',1),('BB','Barbados',1),('BD','Bangladesh',1),('BF','Burkina Faso',1),('BH','Bahrain',1),('BI','Burundi',1),('BJ','Benin',1),('BM','Bermuda',1),('BN','Brunei Darussalam',1),('BO','Bolivia',1),('BR','Brazil',1),('BS','Bahamas',1),('BT','Bhutan',1),('BW','Botswana',1),('BZ','Belize',1),('CA','Canada',1),('CD','Dominican Republic',1),('CF','Central African Republic',1),('CG','Congo (Rep)',1),('CI','Cote d\'Ivoire (Rep)',1),('CK','Cook Islands',1),('CL','Chile',1),('CM','Cameroon',1),('CO','Colombia',1),('CR','Costa Rica',1),('CS','China',0),('CV','Cape Verde',1),('DZ','Algeria',1),('EC','Ecuador',1),('EG','Egypt',1),('ER','Eritrea',1),('ET','Ethiopia',1),('FJ','Fiji',1),('FO','Falkland Islands (Malvinas)',1),('GA','Gabon',1),('GD','Grenada',1),('GH','Ghana',1),('GI','Gibraltar',1),('GN','Guinea',1),('GT','Guatemala',1),('GY','Guyana',1),('HN','Honduras',1),('HT','Haiti',1),('ID','Indonesia',0),('IN','India',1),('JM','Jamaica',1),('JO','Jordan',1),('JP','Japan',1),('KE','Kenya',1),('KH','Cambodia',1),('KR','Korea (Rep)',1),('KW','Kuwait',1),('KY','Cayman Islands',1),('KZ','Kazakhstan',1),('LA','Lao People\'s Democratic Republic',1),('LB','Lebanon',1),('LC','Saint Lucia',1),('LK','Sri Lanka',1),('LS','Lesotho',1),('MA','Morocco',1),('MG','Madagascar',1),('ML','Mali',1),('MM','Myanmar',1),('MN','Mongolia',1),('MR','Mauritania',1),('MU','Mauritius',1),('MV','Maldives',1),('MW','Malawi',1),('MX','Mexico',1),('MY','Malaysia',0),('NA','Namibia',1),('NC','New Caledonia',1),('NE','Niger',1),('NG','Nigeria',1),('NP','Nepal',1),('NR','Nauru',1),('NZ','New Zealand',1),('OM','Oman',1),('PA','Panama',1),('PE','Peru',1),('PG','Papua New Guinea',1),('PK','Pakistan',1),('PN','Pitcairn Islands',1),('PY','Paraguay',1),('QA','Qatar',1),('SA','Saudi Arabia',1),('SB','Solomon Islands',1),('SC','Seychelles',1),('SD','Sudan',1),('SL','Sierra Leone',1),('SN','Senegal',1),('SO','Somalia',1),('SR','Suriname',1),('ST','Sao Tome and Principe',1),('SV','El Salvador',1),('SY','Syrian Arab Republic',1),('SZ','Swaziland',1),('TD','Chad',1),('TG','Tonga',1),('TH','Thailand',0),('TJ','Tajikistan',1),('TN','Tunisia',1),('TT','Trinidad and Tobago',1),('TZ','Tanzania',1),('UG','Uganda',1),('UY','Uruguay',1),('UZ','Uzbekistan',1),('VC','Saint Vincent and the Grenadines',1),('VE','Venezuela',1),('VN','Vietnam',0),('VU','Vanuatu',1),('WF','Wallis and Futuna',1),('WS','Western Sahara',1),('YE','Yemen',1),('ZA','South Africa',1),('ZM','Zambia',1),('ZW','Zimbabwe',1);
/*!40000 ALTER TABLE `Country` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-02-19 16:06:11
