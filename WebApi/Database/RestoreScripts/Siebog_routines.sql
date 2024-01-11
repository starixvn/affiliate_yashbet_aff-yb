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
-- Dumping routines for database 'Siebog'
--
/*!50003 DROP FUNCTION IF EXISTS `Currval` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` FUNCTION `Currval`(v_seq_name VARCHAR(50)) RETURNS int(11)
BEGIN
 set @value = 0;
 select current_val into @value from Siebog.Sequence where seq_name = v_seq_name;
RETURN @value;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `Nextval` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` FUNCTION `Nextval`(v_seq_name VARCHAR(50)) RETURNS int(11)
BEGIN
   	# 20191125@Scott (MemberSite): Get Sequence for Each request
	SET @current_val=0;
    SET @increment_val=1;
    
	SELECT 
		Sequence.current_val + Sequence.increment_val
	INTO @current_val FROM
		Siebog.Sequence
	WHERE
		Sequence.seq_name = v_seq_name
	FOR UPDATE;
    
    IF @current_val=0 THEN
		SET @current_val = @current_val + @increment_val;
        
		INSERT INTO `Siebog`.`Sequence`
		(`seq_name`,
		`current_val`,
        `increment_val`
        )
		VALUES
		(v_seq_name,
		@current_val,
        @increment_val);
    ELSE
		UPDATE Siebog.Sequence 
		SET 
			current_val = @current_val
		WHERE
			seq_name = v_seq_name;
    END IF;
    
    RETURN @current_val;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `AdjustmentInfoFirstDeposit_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `AdjustmentInfoFirstDeposit_Set`(
	IN AdjustId INT
)
BEGIN
	# 20200114@Scott (BackendSite): AdjustmentInfo set FirstDeposit flag
	UPDATE `Siebog`.`AdjustmentInfo` 
	SET 
		AdjustmentInfo.FirstDeposit = true
	WHERE
		AdjustmentInfo.AdjustId = AdjustId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `AdjustmentLog_Ins` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `AdjustmentLog_Ins`(
	IN AdjustId INT,
    IN SiteId INT,
    IN `Action` INT,
    IN `Comment` VARCHAR(500),
    IN Operator VARCHAR(30)
)
BEGIN
	# 20191126@Scott (BackendSite, Payment Service): to Record DepositLog.
	INSERT INTO `Siebog`.`AdjustmentLog`
		(`AdjustId`,
		`SiteId`,
		`Action`,
		`Comment`,
        Operator
        )
	VALUES 
		(AdjustId,
		SiteId,
		`Action`,
		`Comment`,
        Operator
		);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Adjustment_Ins` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Adjustment_Ins`(
	IN AdjustType TINYINT, #Debit = 1, Credit = 2, Bonus = 3,
    IN CustId INT,
    IN UserName VARCHAR(30),
    IN CurrencyId TINYINT,
    IN SiteId INT ,
    IN AdjustIdInfo JSON,
    IN Amount DECIMAL(18,4),
    IN Prefix CHAR(2),
    OUT TransId VARCHAR(30)
)
BEGIN
    # 20191212@Scott (Backend): PaymentAdjustment.
	SET @AdjustId = 0;

	INSERT INTO AdjustmentInfo(AdjustType,CustId,UserName,CurrencyId,SiteId,AdjustIdInfo,Amount, RefId , Memo,`Comment`)
	VALUES(AdjustType, CustId, UserName, CurrencyId, SiteId, AdjustIdInfo, Amount, RefId , Memo, `Comment`);
    
    IF ROW_COUNT()> 0 THEN
		SET @AdjustId=LAST_INSERT_ID();
		SET @TransId=CONCAT(Prefix, @AdjustId);
		UPDATE AdjustmentInfo 
		SET 
			AdjustmentInfo.TransId = @TransId
		WHERE
			AdjustmentInfo.AdjustId = @AdjustId;
			
		SET TransId = @TransId;
    END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Adjustment_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Adjustment_Set`(
	IN AdjustId INT,
    IN SiteId INT,
    IN RefId VARCHAR(100),
    IN `Comment` VARCHAR(500),
    IN ForStatus JSON,
    IN `Status` TINYINT,
    OUT CustId INT,
    OUT UserName VARCHAR(30)
)
BEGIN
	# 20200114@Scott (BackendSite): AdjustmentInfo to Set Status.
    
    SET @AdjustId=0;

	SELECT 
		AdjustmentInfo.CustId, AdjustmentInfo.UserName, AdjustmentInfo.AdjustId
	INTO CustId , UserName, @AdjustId FROM
		AdjustmentInfo
	WHERE
		AdjustmentInfo.AdjustId = AdjustId
			AND JSON_CONTAINS(ForStatus , CONVERT(AdjustmentInfo.`Status`,CHAR))=1
			AND AdjustmentInfo.SiteId = SiteId
	FOR UPDATE NOWAIT;
    
    IF @AdjustId<>0 THEN
		UPDATE `Siebog`.`AdjustmentInfo` 
		SET 
			AdjustmentInfo.`Status` = IF(`Status` = 0,
				`AdjustmentInfo`.`Status`,
				`Status`),
			AdjustmentInfo.`Comment` = `Comment`,
			AdjustmentInfo.RefId = RefId
		WHERE
			AdjustmentInfo.AdjustId = @AdjustId;
	END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `AdminAccountChangePwd_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `AdminAccountChangePwd_Set`(
    IN SiteId INT,
    IN UserId INT,
    IN UserPwd varchar(50)
)
BEGIN
	# 20191209@Eddie (BackendSite): Change Admin Account pwd
	UPDATE AdminUser
	SET 
		AdminUser.`UserPwd` = `UserPwd`
	WHERE
		AdminUser.UserId = UserId
			AND AdminUser.SiteId = SiteId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `AdminAccountChangeSelfPwd_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `AdminAccountChangeSelfPwd_Set`(
    IN SiteId INT,
    IN UserId INT,
    IN Md5OldPassword varchar(50),
	IN Md5NewPassword varchar(50),
    OUT Md5Userpwd varchar(50)
)
BEGIN
	# 20191209@Eddie (BackendSite): Change Admin Account pwd
 	SELECT 
		AdminUser.Userpwd
	INTO Md5Userpwd FROM
		AdminUser
	WHERE
		AdminUser.UserId = UserId
			AND AdminUser.SiteId = SiteId
	FOR UPDATE;

	UPDATE AdminUser 
	SET 
		AdminUser.Userpwd = Md5NewPassword
	WHERE
		AdminUser.UserId = UserId
			AND AdminUser.Userpwd = Md5OldPassword
            AND AdminUser.SiteId = SiteId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `AdminAccountId_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `AdminAccountId_Get`(
	IN SiteId INT,
    In UserId INT
)
BEGIN
	# 20191209@Eddie(BackendSite): Get Admin Account by UserId
	SELECT 
		UserId,
		UserName,
        `Role`,
        `Status`,
        CreateTime
	FROM
		AdminUser
	WHERE
		AdminUser.SiteId = SiteId AND
        AdminUser.UserId = UserId
	;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `AdminAccountId_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `AdminAccountId_Set`(
    IN SiteId INT,
    IN UserId INT,
    IN `Role` JSON,
    IN `Status` tinyint(4)
)
BEGIN
	# 20191209@Eddie (BackendSite): Edie Admin Account
	UPDATE AdminUser
	SET 
		AdminUser.`Role` = `Role`,
        AdminUser.`Status` = `Status`
	WHERE
		AdminUser.UserId = UserId
			AND AdminUser.SiteId = SiteId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `AdminAccount_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `AdminAccount_Get`(
	IN SiteId INT
)
BEGIN
	# 20191209@Eddie(BackendSite): Get List of Admin Account
	SELECT 
		UserId,
		UserName,
        `Role`,
        `Status`,
        CreateTime
	FROM
		AdminUser
	WHERE
		AdminUser.SiteId = SiteId
	;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `AdminAccount_Ins` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `AdminAccount_Ins`(
	IN UserName VARCHAR(30),
    IN UserPwd VARCHAR(50),
    IN `Role` VARCHAR(50),
    IN SiteId INT,
    OUT UserId INT
)
BEGIN
	# 20191209@Eddie(BackendSite): Insert Admin Account.
	INSERT INTO AdminUser(SiteId, UserName, UserPwd, `Role`)
	SELECT 
		SiteId, UserName,UserPwd,`Role`
	FROM DUAL WHERE
		NOT EXISTS( SELECT 
				1
			FROM
				AdminUser
			WHERE
				AdminUser.UserName = UserName
					AND AdminUser.SiteId = SiteId);
                    
	SET UserId = LAST_INSERT_ID();
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Admin_AdjustmentInfoByCust_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Admin_AdjustmentInfoByCust_Get`(
	IN CustId INT,
    IN TransId INT,    
    IN StartDate BIGINT,
    IN EndDate BIGINT,    
	IN PageSize INT,
    INOUT PageNumber INT,
    OUT TotalPages INT,
    OUT RowCount INT,
    OUT StartIndex INT    
)
BEGIN
   	# 20191125@Eddie Wu (BackendSite): Get List of WithdrawInfo
    # 20200104@Scott (BackendSite): Add conditions
    
	SET @StartDate = FROM_UNIXTIME(StartDate);
    SET @EndDate = FROM_UNIXTIME(EndDate);
    
	SELECT 
		COUNT(1)
	INTO RowCount FROM
		AdjustmentInfo AS AI
	WHERE
		AI.CreateTime BETWEEN @StartDate AND @EndDate
		AND AI.CustId = CustId
		AND (AI.TransId = IFNULL(TransId, AI.TransId));
            
    	IF RowCount = 0 THEN 
			SET PageNumber = 1;
			SET TotalPages = 0;
		ELSE
			SET TotalPages = CEIL(RowCount/PageSize);		
			IF PageNumber > TotalPages THEN 
				SET PageNumber = TotalPages;
			END IF;
			
			SET StartIndex = (PageNumber - 1) * PageSize; 

			SELECT 
				AI.AdjustId,
				AI.TransId,
				AI.AdjustType,
				AI.SiteId,
				AI.Amount,
				DATE_FORMAT(AI.CreateTime, '%Y-%m-%dT%TZ') AS CreateTime,
				AI.`Status`
			FROM
				AdjustmentInfo AS AI
			WHERE
				AI.CreateTime BETWEEN @StartDate AND @EndDate
                AND AI.CustId = CustId
				AND (AI.TransId = IFNULL(TransId, AI.TransId))
			ORDER BY AI.AdjustId DESC
			LIMIT StartIndex , PageSize;
		END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Admin_CustInfoDetailSearch_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Admin_CustInfoDetailSearch_Get`(
	IN CustId INT,
    IN UserName VARCHAR(30)
    #IN SiteId INT
)
BEGIN
	# 20191213@Scott (BackendSite): Get CustInfo for Cust Manage

	SELECT 
		SI.SiteName,
		C.CustId,
		C.UserName,
		C.CurrencyId,
		C.UserLevel,
		A.UserName AS Agent,
        C.UserLang,
        C.ActStatus,
		C.IsTest,
        C.IsKyc,
        C.IsRisk,
		C.`Status`,
        DATE_FORMAT(C.LastLoginTime, '%Y-%m-%dT%TZ') AS LastLoginTime,
        DATE_FORMAT(C.RegTime, '%Y-%m-%dT%TZ') AS RegTime,
        P.Email,
        P.`Profile`,
        IFNULL(W.Balance, 0) AS Balance,
        DATE_FORMAT(W.FirstDepositTime, '%Y-%m-%dT%TZ') AS FirstDepositTime,
        IFNULL(CTS.DepositSum, 0) AS DepositSum,
        IFNULL(CTS.WithdrawlSum, 0) AS WithdrawlSum,
        IFNULL(CTS.EffectiveBetSum, 0) AS EffectiveBetSum,
        IF(CPI.PicFileName IS NULL, 0, 1) AS HasPhotoId,
        CBI.AccountName,        
        P.`Profile`->>'$.CountryCode' AS CountryCode,
        P.AreaCode,
        P.PhoneNo        
	FROM
		Customer AS C
			INNER JOIN
		 ServerInfo AS SI ON C.SiteId = SI.SiteId
			LEFT OUTER JOIN
		Customer AS A ON C.AgentId = A.CustId
			LEFT OUTER JOIN
		CustProfile AS P ON C.CustId = P.CustId
			LEFT OUTER JOIN
		CustWallet AS W ON C.CustId = W.CustId
			LEFT OUTER JOIN
		CustTransSummary AS CTS ON C.CustId = CTS.CustId        
			LEFT OUTER JOIN
		CustPhotoId AS CPI ON C.CustId = CPI.CustId
			LEFT OUTER JOIN
		CustBankInfo AS CBI ON C.CustId = CBI.CustId        
	WHERE
		C.CustId = CustId OR C.UserName = UserName/*AND C.SiteId = SiteId*/;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Admin_CustInfoSearchForExport_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Admin_CustInfoSearchForExport_Get`(
	IN UserName VARCHAR(30),
    IN TpUserName VARCHAR(50),
	IN TransId VARCHAR(15)
)
BEGIN
	# 20200205@Scott (BackendSite): CustInfo For Export
           
	SELECT 
		SI.SiteName,
		C.CustId,
		C.UserName,
		C.CurrencyId,                
		C.UserLevel,
		A.UserName AS Agent,
        CBI.AccountName AS RealName,
        P.`Profile`->>'$.CountryCode' AS CountryCode,
        P.AreaCode,
        P.PhoneNo,        
		P.Email,
		C.IsTest,
		C.`Status`,
		DATE_FORMAT(C.RegTime, '%Y-%m-%dT%TZ') AS RegTime,
		TCI.TpUserName AS SabaID
	FROM
		Customer AS C LEFT OUTER JOIN
		Customer AS A ON C.AgentId = A.CustId
			LEFT OUTER JOIN
		ServerInfo AS SI ON SI.SiteId = C.SiteId
			LEFT OUTER JOIN
		TpCustInfo AS TCI ON TCI.CustId = C.CustId AND TCI.TpId=1
			LEFT OUTER JOIN
		CustProfile AS P ON C.CustId = P.CustId
			LEFT OUTER JOIN
		CustBankInfo AS CBI ON C.CustId = CBI.CustId          
	WHERE
		(C.UserName LIKE IF(UserName IS NULL, '', CONCAT('%',UserName,'%'))
			OR EXISTS( SELECT 
				1
			FROM
				DepositInfo
			WHERE
				DepositInfo.TransId = TransId
					AND DepositInfo.CustId = C.CustId)
			OR EXISTS( SELECT 
				1
			FROM
				WithdrawalInfo
			WHERE
				WithdrawalInfo.TransId = TransId
					AND WithdrawalInfo.CustId = C.CustId)
			OR EXISTS( SELECT 
				1
			FROM
				TpTransferInfo
			WHERE
				TpTransferInfo.TransId = TransId
					AND TpTransferInfo.CustId = C.CustId)
			OR EXISTS(SELECT 
				1
			FROM
				TpCustInfo
			WHERE
				TpCustInfo.TpUserName = TpUserName
					AND TpCustInfo.CustId = C.CustId));
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Admin_CustInfoSearch_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Admin_CustInfoSearch_Get`(
	IN UserName VARCHAR(30),
    IN TpUserName VARCHAR(50),
	IN TransId VARCHAR(15),
	IN PageSize INT,
    INOUT PageNumber INT,
    OUT TotalPages INT,
    OUT RowCount INT,    
    OUT StartIndex INT
)
BEGIN
	# 20191213@Scott (BackendSite): Get CustInfo for Cust Manage
    # 20200129@Scott (BackendSite): Add Reg Time for List
    # 20200130@Scott (BackendSite): Add TpUserName for Condition
    
	SET PageSize = ABS(PageSize);
    SET PageNumber = ABS(PageNumber);
    SET RowCount=0;
    
    IF UserName IS NOT NULL OR TransId IS NOT NULL OR TpUserName IS NOT NULL THEN
		SELECT 
			COUNT(1)
		INTO RowCount FROM
			Customer
		WHERE
			(Customer.UserName LIKE IF(UserName IS NULL,
				'',
				CONCAT('%', UserName, '%'))
				OR EXISTS(SELECT 
					1
				FROM
					DepositInfo
				WHERE
					DepositInfo.TransId = TransId
						AND DepositInfo.CustId = Customer.CustId)
				OR EXISTS(SELECT 
					1
				FROM
					WithdrawalInfo
				WHERE
					WithdrawalInfo.TransId = TransId
						AND WithdrawalInfo.CustId = Customer.CustId)
				OR EXISTS(SELECT 
					1
				FROM
					TpTransferInfo
				WHERE
					TpTransferInfo.TransId = TransId
						AND TpTransferInfo.CustId = Customer.CustId)
				OR EXISTS(SELECT 
					1
				FROM
					TpCustInfo
				WHERE
					TpCustInfo.TpUserName = TpUserName
						AND TpCustInfo.CustId = Customer.CustId));
    
    	IF RowCount = 0 THEN 
			SET PageNumber = 1;
			SET TotalPages = 0;
		ELSE
			SET TotalPages = CEIL(RowCount/PageSize);		
			IF PageNumber > TotalPages THEN 
				SET PageNumber = TotalPages;
			END IF;
			
			SET StartIndex = (PageNumber - 1) * PageSize; 
            
			SELECT 
				SI.SiteName,
				C.CustId,
				C.UserName,
                C.CurrencyId,                
                C.UserLevel,
                A.UserName AS Agent,
                C.IsTest,
                C.`Status`,
                DATE_FORMAT(C.RegTime, '%Y-%m-%dT%TZ') AS RegTime,
                TCI.TpUserName AS SabaID
			FROM
				Customer AS C LEFT OUTER JOIN
				Customer AS A ON C.AgentId = A.CustId
					LEFT OUTER JOIN
                ServerInfo AS SI ON SI.SiteId = C.SiteId
					LEFT OUTER JOIN
                TpCustInfo AS TCI ON TCI.CustId = C.CustId AND TCI.TpId=1
			WHERE
				(C.UserName LIKE IF(UserName IS NULL, '', CONCAT('%',UserName,'%'))
					OR EXISTS( SELECT 
						1
					FROM
						DepositInfo
					WHERE
						DepositInfo.TransId = TransId
							AND DepositInfo.CustId = C.CustId)
					OR EXISTS( SELECT 
						1
					FROM
						WithdrawalInfo
					WHERE
						WithdrawalInfo.TransId = TransId
							AND WithdrawalInfo.CustId = C.CustId)
					OR EXISTS( SELECT 
						1
					FROM
						TpTransferInfo
					WHERE
						TpTransferInfo.TransId = TransId
							AND TpTransferInfo.CustId = C.CustId)
					OR EXISTS(SELECT 
						1
					FROM
						TpCustInfo
					WHERE
						TpCustInfo.TpUserName = TpUserName
							AND TpCustInfo.CustId = C.CustId))
                    LIMIT StartIndex , PageSize;
			END IF;
		END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Admin_CustNotes_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Admin_CustNotes_Get`(
	IN CustId INT
)
BEGIN
	# 20200117@Scott (BackendSite): get Notes for Cust.
	SELECT 
		`Id`, 
        `CustId`, 
        `Note`, 
        DATE_FORMAT(CreateTime, '%Y-%m-%dT%TZ') AS CreateTime, 
        `Operator`
	FROM
		`Siebog`.`CustNotes`
	WHERE
		CustNotes.CustId = CustId
	ORDER BY Id DESC;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Admin_CustNotes_Ins` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Admin_CustNotes_Ins`(
	IN CustId INT,
    IN `Note` VARCHAR(500),
    IN Operator VARCHAR(30)
)
BEGIN
	# 20200117@Scott (BackendSite): Insert Note for Cust.
		INSERT INTO `CustNotes`
		(`CustId`,
		`Note`,
		`Operator`)
		VALUES
		(CustId,
		Note,
		Operator);

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Admin_CustPhotoId_Del` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Admin_CustPhotoId_Del`(
	IN CustId INT
)
BEGIN
	# 20200116@Scott (Admin): to Delete Submitted PhotoId.
    
	DELETE FROM Siebog.CustPhotoId 
	WHERE
		CustPhotoId.CustId = CustId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Admin_CustPhotoId_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Admin_CustPhotoId_Get`(
	IN CustId INT
)
BEGIN
	# 20200116@Scott (BackendSite): to Set Cust Status.
	SELECT 
		CustPhotoId.CustId,
		CustPhotoId.PicFileName,
		CustPhotoId.PhotoIdPic
	FROM
		CustPhotoId
	WHERE
		CustPhotoId.CustId = CustId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Admin_CustStatusSwitch_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Admin_CustStatusSwitch_Set`(
	IN CustId INT,
    #IN SiteId INT,
    IN Op TINYINT
)
BEGIN
	# 20200116@Scott (Backend): to Set Cust Status.
	
    IF Op=1 THEN
		UPDATE Customer 
		SET 
			Customer.IsKyc = NOT Customer.IsKyc
		WHERE
			Customer.CustId = CustId;
    ELSEIF Op=2 THEN
		UPDATE Customer 
		SET 
			Customer.IsRisk = NOT Customer.IsRisk
		WHERE
			Customer.CustId = CustId;
    END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Admin_DepositInfoListByCust_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Admin_DepositInfoListByCust_Get`(
    IN CustId INT,
	IN TransId VARCHAR(30),
    IN StartDate BIGINT,
    IN EndDate BIGINT,
	IN PageSize INT,
    INOUT PageNumber INT,
    OUT TotalPages INT,
    OUT RowCount INT,
    OUT StartIndex INT
)
BEGIN
	# 20200117@Scott (Backend): Get DepositInfo for CustInfo
    
	SET PageSize = ABS(PageSize);
    SET PageNumber = ABS(PageNumber);
    SET RowCount=0;
    #FROM_UNIXTIME
    SET @StartDate = FROM_UNIXTIME(StartDate);
    SET @EndDate = FROM_UNIXTIME(EndDate);
    IF StartDate IS NOT NULL AND EndDate IS NOT NULL THEN
		SELECT 
			COUNT(1)
		INTO RowCount FROM
			DepositInfo
		WHERE
			(DepositInfo.CreateTime BETWEEN @StartDate AND @EndDate)
				#AND DepositInfo.TransId=IFNULL(TransId, DepositInfo.TransId)
				AND DepositInfo.CustId=CustId;
    
    	IF RowCount = 0 THEN 
			SET PageNumber = 1;
			SET TotalPages = 0;
		ELSE
			SET TotalPages = CEIL(RowCount/PageSize);		
			IF PageNumber > TotalPages THEN 
				SET PageNumber = TotalPages;
			END IF;
			
			SET StartIndex = (PageNumber - 1) * PageSize; 
            
			SELECT 
                DATE_FORMAT(DI.CreateTime, '%Y-%m-%dT%TZ') AS CreateTime,            
                DI.DepositType,                 
				DI.TransId, 
                DI.DepositInfo, 
                DI.Amount, 
                DI.`Status`,
				DI.RefId,
				DATE_FORMAT(DI.DepositTime, '%Y-%m-%dT%TZ') AS DepositTime,
				DI.DepositInfo->>'$.AccountName' AS AccountName,            
				DI.DepositInfo->>'$.AccountNo' AS AccountNo       
			FROM
				DepositInfo AS DI LEFT OUTER JOIN
				DepositReceipt AS DR ON DI.DepositId = DR.DepositId
			WHERE
				(DI.CreateTime BETWEEN @StartDate AND @EndDate)
					#AND DepositInfo.TransId=IFNULL(TransId, DepositInfo.TransId)
					AND DI.CustId=CustId
                    ORDER BY DI.DepositId DESC
                    LIMIT StartIndex , PageSize;
		END IF;
	END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Admin_DepositListForExport_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Admin_DepositListForExport_Get`(
    IN DepositType INT,
    IN StartDate BIGINT,
    IN EndDate BIGINT,    
    IN DepositId INT,
	IN TransId VARCHAR(30),
    IN UserName VARCHAR(30),
    IN CurrencyId TINYINT,
    IN `Status` INT
)
BEGIN
    # 20200121@Scott (BackendSite): For Export
    
	SET @StartDate = FROM_UNIXTIME(StartDate);
    SET @EndDate = FROM_UNIXTIME(EndDate);
		
	SELECT 
		DI.DepositId,
		SI.SiteName,         
		DI.TransId,
		DI.DepositType,
		DI.CurrencyId,
		DI.CustId,
		DI.UserName,
		DI.Amount,
		DATE_FORMAT(DI.CreateTime, '%Y-%m-%dT%TZ') AS CreateTime,
        DATE_FORMAT(IF(DI.DepositType=3, DI.DepositTime, DI.CallbackTime), '%Y-%m-%dT%TZ') AS DepositTime,
		DI.RefId,
		DI.FirstDeposit,        
		DI.`Status`,
		DI.DepositInfo->>'$.AccountName' AS AccountName,            
		DI.DepositInfo->>'$.AccountNo' AS AccountNo,
		BIS.BankCode,
        PL.PayName
	FROM
		DepositInfo AS DI 
		INNER JOIN 
		BankInfoSys AS BIS ON DI.DepositInfo->'$.SysId'= BIS.Id
		INNER JOIN 
		ServerInfo AS SI ON SI.SiteId = DI.SiteId
        LEFT OUTER JOIN 
		PayList AS PL ON DI.DepositInfo->'$.PayId'= PL.Id        
	WHERE
		DI.DepositType = IFNULL(DepositType, DI.DepositType)
		AND DI.CreateTime BETWEEN @StartDate AND @EndDate
		AND (DI.TransId = IFNULL(TransId, DI.TransId)
		OR DI.DepositId = IFNULL(DepositId, DI.DepositId))
		AND DI.UserName = IFNULL(UserName, DI.UserName)
		AND DI.CurrencyId = IFNULL(CurrencyId, DI.CurrencyId)
		AND DI.`Status` = IFNULL(`Status`, DI.`Status`)
		ORDER BY DI.CreateTime;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Admin_DepositRequestTransID_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Admin_DepositRequestTransID_Get`(
	IN DepositId INT
    #IN SiteId INT
)
BEGIN
   	# 20191125@Eddie Wu (BackendSite): Get DepositInfo by DepositId
    # 20200113@Scott (BackendSite): Added AdjustFlag
    # 20200127@Scott (BackendSite): Added Real Name(Account Name)
    
	SELECT DI.DepositId,
		DI.TransId,
		DI.DepositType,
		DI.CustId,
		DI.UserName,
		CP.Email,
        CBI.AccountName AS RealName,
        DI.CurrencyId,
		DI.SiteId,
        SI.SiteName,
		DI.DepositInfo,
		DI.Amount,
		DI.ActualDeposit,
        DI.`Comment`,
		DATE_FORMAT(DI.CreateTime, '%Y-%m-%dT%TZ') AS CreateTime,
		DATE_FORMAT(DI.TPCreateTime, '%Y-%m-%dT%TZ') AS TPCreateTime,
		DATE_FORMAT(DI.CallbackTime, '%Y-%m-%dT%TZ') AS CallbackTime,
		DI.FirstDeposit,
		DI.Memo,
		#DI.PostScript,
        DI.RefId,
        DATE_FORMAT(IF(DI.DepositType=3, DI.DepositTime, DI.CallbackTime), '%Y-%m-%dT%TZ') AS DepositTime,
		DI.`Status`,
        BIS.BankName,
        BIS.BankCode,        
		DI.DepositInfo->>'$.AccountName' AS AccountName,            
		DI.DepositInfo->>'$.AccountNo' AS AccountNo,        
        PL.PayName,
        IF(DR.DepositPic IS NULL, 0, 1) AS Receipt,
        DR.PicFileName
	FROM
		DepositInfo AS DI LEFT OUTER JOIN
		DepositReceipt AS DR ON DI.DepositId = DR.DepositId
		INNER JOIN 
        BankInfoSys AS BIS ON DI.DepositInfo->'$.SysId'= BIS.Id        
        INNER JOIN 
		ServerInfo AS SI ON SI.SiteId = DI.SiteId
        LEFT OUTER JOIN 
		PayList AS PL ON DI.DepositInfo->'$.PayId'= PL.Id
        LEFT OUTER JOIN 
        CustProfile AS CP ON CP.CustId = DI.CustId
        LEFT OUTER JOIN 
        CustBankInfo AS CBI ON DI.CustId = CBI.CustId              
	WHERE
		DI.DepositId = DepositId;
			#AND DI.SiteId = SiteId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Admin_FinancialLogList_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Admin_FinancialLogList_Get`(
    IN CustId INT,    
    IN StartDate BIGINT,
    IN EndDate BIGINT,
	IN PageSize INT,
    INOUT PageNumber INT,
    OUT TotalPages INT,
    OUT RowCount INT,
    OUT StartIndex INT
)
BEGIN
	# 20200116@Scott (Backend): Get FinancialLog
	SET PageSize = ABS(PageSize);
    SET PageNumber = ABS(PageNumber);
    SET RowCount=0;
    #FROM_UNIXTIME
    SET @StartDate = FROM_UNIXTIME(StartDate);
    SET @EndDate = FROM_UNIXTIME(EndDate);
    IF StartDate IS NOT NULL AND EndDate IS NOT NULL THEN
		SELECT 
			COUNT(1)
		INTO RowCount FROM
			FinancialLog
		WHERE
			(FinancialLog.CreateTime BETWEEN @StartDate AND @EndDate)
				AND FinancialLog.CustId=CustId;
    
    	IF RowCount = 0 THEN 
			SET PageNumber = 1;
			SET TotalPages = 0;
		ELSE
			SET TotalPages = CEIL(RowCount/PageSize);		
			IF PageNumber > TotalPages THEN 
				SET PageNumber = TotalPages;
			END IF;
			
			SET StartIndex = (PageNumber - 1) * PageSize; 
            
			SELECT 
				FL.Id,
                DATE_FORMAT(FL.CreateTime, '%Y-%m-%dT%TZ') AS CreateTime,            
                FT.TypeName,                 
				FL.Direction, 
                FL.ChangeAmount, 
                FL.BeforeAmount, 
                FL.AfterAmount, 
				FL.RefTransId,
                FL.Memo           
			FROM
				FinancialLog AS FL LEFT OUTER JOIN
                FinancialType AS FT ON FL.`Type` = FT.`Type`
			WHERE
				(FL.CreateTime BETWEEN @StartDate AND @EndDate)
					AND FL.CustId=CustId
                    ORDER BY FL.Id DESC
                    LIMIT StartIndex , PageSize;
		END IF;
	END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Admin_LoginLogByCust_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Admin_LoginLogByCust_Get`(
	IN CustId INT,
    IN StartDate BIGINT,
    IN EndDate BIGINT,    
	IN PageSize INT,
    INOUT PageNumber INT,
    OUT TotalPages INT,
    OUT RowCount INT,
    OUT StartIndex INT    
)
BEGIN
   	# 20200117@Scott Wang (BackendSite): Get List of LoginLog
    
	SET @StartDate = FROM_UNIXTIME(StartDate);
    SET @EndDate = FROM_UNIXTIME(EndDate);
    
	SELECT 
		COUNT(1)
	INTO RowCount FROM
		LoginLog
	WHERE
		LoginLog.LoginTime BETWEEN @StartDate AND @EndDate
			AND LoginLog.CustId = CustId;
            
    	IF RowCount = 0 THEN 
			SET PageNumber = 1;
			SET TotalPages = 0;
		ELSE
			SET TotalPages = CEIL(RowCount/PageSize);		
			IF PageNumber > TotalPages THEN 
				SET PageNumber = TotalPages;
			END IF;
			
			SET StartIndex = (PageNumber - 1) * PageSize; 

			SELECT 
				LoginLog.Id,
				DATE_FORMAT(LoginLog.LoginTime, '%Y-%m-%dT%TZ') AS LoginTime,
				LoginLog.LoginIp,
				LoginLog.FromUrl,
				LoginLog.UserAgent
			FROM
				LoginLog
			WHERE
				LoginLog.LoginTime BETWEEN @StartDate AND @EndDate
					AND LoginLog.CustId = CustId
			ORDER BY LoginLog.Id DESC
			LIMIT StartIndex , PageSize;
		END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Admin_ResetCustBankInfo_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Admin_ResetCustBankInfo_Set`(
	IN CustId INT
)
BEGIN
	# 20200131@Scott (Backend): Reset CustBankInfo.
    
	DELETE FROM Siebog.OnUser 
	WHERE
		OnUser.CustId = CustId;

	DELETE FROM Siebog.CustBankInfo
	WHERE
		CustBankInfo.CustId = CustId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Admin_ResetProfile_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Admin_ResetProfile_Set`(
	IN CustId INT
)
BEGIN
	# 20200131@Scott (Backend): Reset Profile.
    
	DELETE FROM Siebog.OnUser 
	WHERE
		OnUser.CustId = CustId;

	UPDATE  CustProfile
	SET 
		CustProfile.`Profile` = NULL,
        CustProfile.PhoneNo = NULL,
        CustProfile.AreaCode = NULL
	WHERE
		CustProfile.CustId = CustId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Admin_ResetPwd_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Admin_ResetPwd_Set`(
	IN CustId INT,
    IN Md5NewPassword VARCHAR(50)
)
BEGIN
	# 20200120@Scott (Backend): Reset Password.
    # 20200120@Scott (Backend): Added kick Out User Process.

	DELETE FROM Siebog.OnUser 
	WHERE
		OnUser.CustId = CustId;

	UPDATE Customer 
	SET 
		Customer.Userpwd = Md5NewPassword
	WHERE
		Customer.CustId = CustId;
        
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Admin_TpTransferInfoByCust_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Admin_TpTransferInfoByCust_Get`(
    IN CustId INT,
	IN TransId VARCHAR(30),
    IN StartDate BIGINT,
    IN EndDate BIGINT,
	IN PageSize INT,
    INOUT PageNumber INT,
    OUT TotalPages INT,
    OUT RowCount INT,
    OUT StartIndex INT
)
BEGIN
	# 20200117@Scott (Backend): Get TpTransferInfo for CustInfo
    
	SET PageSize = ABS(PageSize);
    SET PageNumber = ABS(PageNumber);
    SET RowCount=0;
    #FROM_UNIXTIME
    SET @StartDate = FROM_UNIXTIME(StartDate);
    SET @EndDate = FROM_UNIXTIME(EndDate);
    IF StartDate IS NOT NULL AND EndDate IS NOT NULL THEN
		SELECT 
			COUNT(1)
		INTO RowCount FROM
			TpTransferInfo
		WHERE
			(TpTransferInfo.CreateTime BETWEEN @StartDate AND @EndDate)
				#AND TpTransferInfo.TransId=IFNULL(TransId, TpTransferInfo.TransId)
				AND TpTransferInfo.CustId=CustId;
    
    	IF RowCount = 0 THEN 
			SET PageNumber = 1;
			SET TotalPages = 0;
		ELSE
			SET TotalPages = CEIL(RowCount/PageSize);		
			IF PageNumber > TotalPages THEN 
				SET PageNumber = TotalPages;
			END IF;
			
			SET StartIndex = (PageNumber - 1) * PageSize; 
            
			SELECT 
                DATE_FORMAT(TI.CreateTime, '%Y-%m-%dT%TZ') AS CreateTime,            
                TL.TpName,
				TI.TransId, 
                TI.Direction,
                TI.Amount, 
				TI.ThirdId,
				TI.Memo,
				TI.`Status`
			FROM
				TpTransferInfo AS TI INNER JOIN
				TpList AS TL ON TI.TpId = TL.Id
			WHERE
				(TI.CreateTime BETWEEN @StartDate AND @EndDate)
					#AND TpTransferInfo.TransId=IFNULL(TransId, TpTransferInfo.TransId)
					AND TI.CustId=CustId
                    ORDER BY TI.TransferId DESC
                    LIMIT StartIndex , PageSize;
		END IF;
	END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Admin_WithdrawalListByCust_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Admin_WithdrawalListByCust_Get`(
	IN CustId INT,
    IN TransId INT,    
    IN StartDate BIGINT,
    IN EndDate BIGINT,    
	IN PageSize INT,
    INOUT PageNumber INT,
    OUT TotalPages INT,
    OUT RowCount INT,
    OUT StartIndex INT    
)
BEGIN
   	# 20191125@Eddie Wu (BackendSite): Get List of WithdrawInfo
    # 20200104@Scott (BackendSite): Add conditions
    
	SET @StartDate = FROM_UNIXTIME(StartDate);
    SET @EndDate = FROM_UNIXTIME(EndDate);
    
	SELECT 
		COUNT(1)
	INTO RowCount FROM
		WithdrawalInfo AS WI
	WHERE
		WI.CreateTime BETWEEN @StartDate AND @EndDate
		AND WI.CustId = CustId;
		#AND (WI.TransId = IFNULL(TransId, WI.TransId));
            
    	IF RowCount = 0 THEN 
			SET PageNumber = 1;
			SET TotalPages = 0;
		ELSE
			SET TotalPages = CEIL(RowCount/PageSize);		
			IF PageNumber > TotalPages THEN 
				SET PageNumber = TotalPages;
			END IF;
			
			SET StartIndex = (PageNumber - 1) * PageSize; 

			SELECT 
				WI.WithdrawalId,
				WI.TransId,
				WI.WithdrawalType,
				#WI.CustId,
				#WI.UserName,
                #WI.CurrencyId,
				WI.SiteId,
				#WI.WithdrawalInfo,
				WI.Amount,
				#WI.ChargeFee,
				DATE_FORMAT(CreateTime, '%Y-%m-%dT%TZ') AS CreateTime,
				#WI.Memo,
				#WI.`Comment`,
				WI.`Status`,
				WI.WithdrawalInfo->>'$.AccountNo' AS AccountNo,
				WI.WithdrawalInfo->>'$.AccountName' AS AccountName                
			FROM
				WithdrawalInfo AS WI
			WHERE
				WI.CreateTime BETWEEN @StartDate AND @EndDate
                AND WI.CustId = CustId
				#AND (WI.TransId = IFNULL(TransId, WI.TransId))
			ORDER BY WI.WithdrawalId DESC
			LIMIT StartIndex , PageSize;
		END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Admin_WithdrawalListExport_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Admin_WithdrawalListExport_Get`(
    IN WithdrawalType INT,
    IN StartDate BIGINT,
    IN EndDate BIGINT,    
    IN WithdrawalId INT,
	IN TransId VARCHAR(30),
    IN UserName VARCHAR(30),
    IN CurrencyId TINYINT,
    IN `Status` INT
)
BEGIN
    # 20200121@Scott (BackendSite): WithdrawalList Export
    
	SET @StartDate = FROM_UNIXTIME(StartDate);
    SET @EndDate = FROM_UNIXTIME(EndDate);

	SELECT 
		WI.WithdrawalId,
		WI.TransId,
		SI.SiteName,
		WI.WithdrawalType,
		WI.CustId,
		WI.UserName,
		WI.CurrencyId,
		WI.SiteId,
		WI.WithdrawalInfo,
		WI.Amount,
		BIS.BankName,
        BIS.BankCode,
		WI.WithdrawalInfo->>'$.AccountNo' AS AccountNo,
		WI.WithdrawalInfo->>'$.AccountName' AS AccountName,        
		DATE_FORMAT(CreateTime, '%Y-%m-%dT%TZ') AS CreateTime,
		PL.PayName,
		WI.`Status`,
		BIS2.BankName AS FromBankName,
		BIS2.BankCode AS FromBankCode,
		WI.WithdrawalInfo->>'$.FromAccountNo' AS FromAccountNo,
		WI.WithdrawalInfo->>'$.FromAccountName' AS FromAccountName                        
	FROM
		WithdrawalInfo AS WI
		INNER JOIN 
		ServerInfo AS SI ON SI.SiteId = WI.SiteId
		LEFT OUTER JOIN 
		BankInfoSys AS BIS ON WI.WithdrawalInfo->'$.SysId'= BIS.Id
		LEFT OUTER JOIN 
		BankInfoSys AS BIS2 ON WI.WithdrawalInfo->'$.FromSysId'= BIS2.Id
		LEFT OUTER JOIN 
        PayList AS PL ON WI.WithdrawalInfo->'$.PayId'= PL.Id        
	WHERE
		WI.WithdrawalType = IFNULL(WithdrawalType, WI.WithdrawalType)
		AND WI.CreateTime BETWEEN @StartDate AND @EndDate
		AND (WI.TransId = IFNULL(TransId, WI.TransId)
		OR WI.WithdrawalId = IFNULL(WithdrawalId, WI.WithdrawalId))
		AND WI.UserName = IFNULL(UserName, WI.UserName)
		AND WI.CurrencyId = IFNULL(CurrencyId, WI.CurrencyId)
		AND WI.`Status` = IFNULL(`Status`, WI.`Status`)
	ORDER BY WI.CreateTime;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `AffiliateAgent_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `AffiliateAgent_Get`(
	IN AffiliateId VARCHAR(32),
    IN SiteId INT
)
BEGIN
	# 20191128@Eddie(BackendSite): Get Agent for Affiliate
	SELECT 
		C.CustId,
		C.SiteId,
		C.UserName,
		C.Userpwd,
		C.CurrencyId,
		C.UserLang,
		C.ActStatus,
		C.AgentId,
		C.AllAgentId,
		C.UserLevel,
		C.IsTest
	FROM
		Affiliate A
			INNER JOIN
		Customer C ON A.CustId = C.CustId
			AND A.SiteId = C.SiteId
	WHERE
		A.AffiliateId = AffiliateId
			AND C.SiteId = SiteId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Affiliate_Ins` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Affiliate_Ins`(
	IN AffiliateId VARCHAR(32),
    IN CustId INT,    
    IN SiteId INT
)
BEGIN
	# 20191201@Scott(BackendSite): Insert AffiliateId for Affiliate Agent.
	INSERT INTO Affiliate(AffiliateId, CustId, SiteId)
	SELECT 
		AffiliateId, CustId, SiteId
	FROM DUAL WHERE
		NOT EXISTS( SELECT 
				1
			FROM
				Affiliate
			WHERE
				Affiliate.AffiliateId = AffiliateId
					AND Affiliate.CustId = CustId
					AND Affiliate.SiteId = SiteId);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Balance_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Balance_Get`(
	IN CustId INT
)
BEGIN
	SELECT IFNULL(Balance, 0) AS Balance
	FROM Customer AS CT LEFT OUTER JOIN
	CustWallet AS CW ON CT.CustId = CW.CustId
	WHERE CT.CustId = CustId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `BankAccountOff_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `BankAccountOff_Get`(
	IN SiteId INT
)
BEGIN
	# 20191203@Scott (MemberSite, BackendSite): BankAccountOff 
    
	SELECT 
		BDO.Id AS BankId,
		BDO.BankAccountName AS AccountName,
		BDO.BankAccountNo AS AccountNo,
		BDO.SysId,
		BDO.MinAmount,
		BDO.MaxAmount,
        BDO.PointLevel,
		BDO.Memo,
        BDO.Purpose,
		BDO.Sort AS BSort,
		BIS.CurrencyId,
		BIS.BankCode,
        BIS.BankName,
		BIS.BankLogo,
		BIS.BankSite,
		BIS.Sort,
		1 AS Ver
	FROM
		BankDetailOff BDO
			INNER JOIN
		BankInfoSys BIS ON BDO.SysId = BIS.Id
	WHERE
		BDO.`Status` = 1 AND BIS.`Status` = 1
			AND BDO.SiteId = SiteId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `BankCard_Del` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `BankCard_Del`(
    IN SiteId INT,
    IN BankId INT
)
BEGIN
	# 20200112@Scott(BackendSite): DELETE Bank Card
	DELETE FROM Siebog.BankDetailOff 
	WHERE
		BankDetailOff.Id = BankId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `BankCard_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `BankCard_Set`(
    IN SiteId INT,
    IN SysId INT,
    IN BankAccountName VARCHAR(50),
    IN BankAccountNo VARCHAR(45),
    IN MinAmount decimal(18, 4),
    IN MaxAmount decimal(18, 4),
    IN DailyLimitAmount decimal(18, 4),
    IN PointLevel INT,
    IN Purpose INT,
    IN `Status` TINYINT,
    IN Sort SMALLINT
)
BEGIN
	# 20200112@Scott(BackendSite): Set Bank Card
	INSERT INTO BankDetailOff(SiteId,SysId,BankAccountName,BankAccountNo,MinAmount,MaxAmount,PointLevel,DailyLimitAmount,Purpose)
	VALUE(SiteId, SysId, BankAccountName, BankAccountNo, MinAmount, MaxAmount, PointLevel, DailyLimitAmount, Purpose)
	ON DUPLICATE KEY 
	UPDATE BankDetailOff.`Status`=`Status`, BankDetailOff.MinAmount=MinAmount, BankDetailOff.MaxAmount=MaxAmount, BankDetailOff.PointLevel=PointLevel, BankDetailOff.DailyLimitAmount=DailyLimitAmount, BankDetailOff.Purpose = Purpose, BankDetailOff.Sort = Sort;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `BankDetailStatus_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `BankDetailStatus_Set`(
    IN SiteId INT,
	IN BankId INT,
    IN `Status` INT    
)
BEGIN
	# 20191129@Eddie (BackendSite): BankDetailOff set Status flag
	UPDATE `Siebog`.`BankDetailOff` 
	SET 
		`BankDetailOff`.`Status` = `Status`
	WHERE
		BankDetailOff.Id = BankId
			AND BankDetailOff.SiteId = SiteId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `BankDetail_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `BankDetail_Get`(
	IN SiteId INT,
	IN PageSize INT,
    INOUT PageNumber INT,
    OUT TotalPages INT,
    OUT RowCount INT,
    OUT StartIndex INT        
)
BEGIN
	# 20191128@Eddie(BackendSite): Get List of BankAccountOff

	SELECT 
		COUNT(1)
	INTO RowCount FROM
		BankDetailOff BDO
			INNER JOIN
		BankInfoSys BIS ON BDO.SysId = BIS.Id
	WHERE
		BIS.`Status` = 1 AND BDO.SiteId = SiteId;
				
	IF RowCount = 0 THEN 
		SET PageNumber = 1;
		SET TotalPages = 0;
	ELSE
		SET TotalPages = CEIL(RowCount/PageSize);		
		IF PageNumber > TotalPages THEN 
			SET PageNumber = TotalPages;
		END IF;
		
		SET StartIndex = (PageNumber - 1) * PageSize; 
            
		SELECT 
			BDO.Id AS BankId,
			BDO.BankAccountName AS AccountName,
			BDO.BankAccountNo AS AccountNo,
			BDO.SysId,
			BDO.MinAmount,
			BDO.MaxAmount,
			BDO.DailyLimitAmount,
			BDO.SupportMethod,
			BDO.PointLevel,
			BDO.Memo,
			BDO.Sort AS BSort,
			BIS.CurrencyId,
			BIS.BankCode,
			BIS.BankName,
			BIS.BankLogo,
			BIS.BankSite,
			BIS.Sort,
			BDO.`Status`
		FROM
			BankDetailOff BDO
				INNER JOIN
			BankInfoSys BIS ON BDO.SysId = BIS.Id
		WHERE
			BIS.`Status` = 1 AND BDO.SiteId = SiteId
		LIMIT StartIndex, PageSize;
	END IF;             
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `BankInfoPay_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `BankInfoPay_Get`(
)
BEGIN
	# 20191203@Scott (MemberSite, BackendSite): BankInfoPay_Get 
    
	SELECT 
		BIS.Id AS SysId,
		BIS.CurrencyId,
		BIS.BankCode AS SysBankCode,
		BIS.BankName AS SysBankName,
		BIS.BankLogo,
		BIP.PayId,
		BIP.BankCode,
		BIP.BankName,
        PC.Id AS ChannelId,
		PC.`Mode`,
		PC.PointLevel,
		PC.PayCode,
        PL.PayName,
        PL.Sort AS MSort,
        PC.Sort AS SSort
	FROM
		BankInfoSys AS BIS
			INNER JOIN
		BankInfoPay AS BIP ON BIP.SysId = BIS.Id
			INNER JOIN
		PayChannel AS PC ON PC.PayId = BIP.PayId
			INNER JOIN
		PayList AS PL ON PC.PayId = PL.Id
	WHERE
		PL.`Status` = 1 AND PC.`Status` = 1
			AND PC.`Type` = 1;
	#ORDER BY PL.Sort, PC.Sort;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `BankInfoSysListForCtrl_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `BankInfoSysListForCtrl_Get`()
BEGIN
    # 20191217@Scott (MemBetSite):  to Get List of BankInfoSys for Customer.
	SELECT 
		BankInfoSys.Id AS SysId,
		BankInfoSys.CurrencyId,
		BankInfoSys.BankCode,
		BankInfoSys.BankName,
		BankInfoSys.BankLogo,
		BankInfoSys.BankSite
	FROM
		BankInfoSys
	WHERE
		BankInfoSys.`Status` = 1
	ORDER BY BankInfoSys.Sort;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `BankInfoSysList_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `BankInfoSysList_Get`(
	IN SiteId INT
)
BEGIN
	# 20191128@Scott (MemBetSite): to Get List of BankInfoSys.
    # 20191217@Scott (MemBetSite): Remark Status for System Use.
	SELECT 
		BankInfoSys.Id AS SysId,
		BankInfoSys.CurrencyId,
		BankInfoSys.BankCode,
		BankInfoSys.BankName,
		BankInfoSys.BankLogo,
		BankInfoSys.BankSite
	FROM
		BankInfoSys
	/*WHERE
		`BankInfoSys`.`Status` = 1*/
	ORDER BY BankInfoSys.Sort;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `BankOffDummy_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `BankOffDummy_Get`(
	IN SiteId INT, 
    IN SysId INT,
    IN AccountNo VARCHAR(45),
    IN CurrencyId int,
    IN Amount DECIMAL
)
BEGIN
	# 20191227@Scott (Backend): Get Dummy for Local Bank

	SELECT 
		SUM(BOD.Amount) AS Dummy
	FROM
		BankOffDummy AS BOD
	WHERE
		BOD.SiteId = SiteId
			AND BOD.SysId = SysId
			AND BOD.AccountNo = AccountNo
			AND BOD.CurrencyId = CurrencyId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `BankOffDummy_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `BankOffDummy_Set`(
	IN SiteId INT, 
    IN SysId INT,
    IN AccountNo VARCHAR(45),
    IN CurrencyId int,
    IN Amount DECIMAL
)
BEGIN
	# 20191227@Scott (Backend): Set Dummy for Local Bank

	INSERT INTO BankOffDummy(SiteId, SysId, AccountNo, CurrencyId, Amount) VALUE(SiteId, SysId, AccountNo, CurrencyId, Amount) 
	ON DUPLICATE KEY 
	UPDATE BankOffDummy.Amount=BankOffDummy.Amount + Amount;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Common_CountryList_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Common_CountryList_Get`()
BEGIN
	SELECT 
		`Code`, 
        Sort
	FROM
		`Siebog`.Country
	order by Sort,`Code`;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Common_ReachedRollover_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Common_ReachedRollover_Get`(
	IN CustId INT
)
BEGIN
	SET @ReachedRollover=TRUE;
    SET @Lack=0;
    
	SELECT 
		FALSE, CustRollOver.RollOver - CustRollOver.TurnOver
	INTO @ReachedRollover,@Lack FROM
		CustRollOver
	WHERE
		CustRollOver.CustId = CustId
			AND CustRollOver.RollOverType = 2
			AND CustRollOver.`Status` = 1
	LIMIT 1
	FOR UPDATE;
    
	SELECT @ReachedRollover AS ReachedRollover, @Lack AS Lack;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Common_RollOverTurnOver_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Common_RollOverTurnOver_Set`(
	IN TpId INT,
	IN CustId INT,
    IN UserName VARCHAR(30),
    IN SiteId INT,
	IN TurnOver DECIMAL(18,2),
	IN Memo VARCHAR(200),
    IN RefTransId VARCHAR(45)    
)
BEGIN
	# 20200210@Scott (MemberSite, Service, Backend): Set TurnOver of RollOver
    
    SET @RollOver = 0;
	SET @TurnOver = 0;    
    SET @BonusCode = '';
    
    SELECT 
		CustRollOver.RollOver,
		CustRollOver.TurnOver,
		CustRollOver.BonusCode
	INTO @RollOver , @TurnOver , @BonusCode FROM
		CustRollOver
	WHERE
		CustRollOver.CustId = CustId
			AND CustRollOver.RollOverType = 2
			AND CustRollOver.`Status` = 1
	LIMIT 1
	FOR UPDATE;
    
    IF @RollOver > 0 THEN 
		IF (@RollOver - @TurnOver) <= TurnOver THEN
			UPDATE CustRollOver 
			SET 
				CustRollOver.TurnOver = CustRollOver.TurnOver + TurnOver,
                CustRollOver.`Status` = 2
			WHERE
				CustRollOver.CustId = CustId
					AND CustRollOver.RollOverType = 2
					AND CustRollOver.BonusCode = @BonusCode;
                    
			UPDATE BonusInfo
			SET 
				BonusInfo.`Status` = 4
			WHERE
				BonusInfo.CustId = CustId
					AND BonusInfo.`Status` = 1
					AND BonusInfo.BonusCode = @BonusCode;
		ELSE
			UPDATE CustRollOver 
			SET 
				CustRollOver.TurnOver = CustRollOver.TurnOver + TurnOver
			WHERE
				CustRollOver.CustId = CustId
					AND CustRollOver.RollOverType = 2
					AND CustRollOver.BonusCode = @BonusCode;
        END IF;
        
		INSERT INTO TurnOverLog
		(TurnOverType, CustId, UserName, SiteId, TpId, MainType, ChangeTurnOver, BeforeTurnOver, AfterTurnOver, Memo, RefTransId)
		VALUES
		(2, CustId, UserName, SiteId, TpId, 1, TurnOver, @TurnOver, @TurnOver + TurnOver, Memo, RefTransId);        
    END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Common_RollOver_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Common_RollOver_Get`(
	IN CustId INT,
	IN BonusCode VARCHAR(50)
)
BEGIN
    SET @RollOver = 0;
    SET @TurnOver = 0;

	SELECT 
		CustRollOver.RollOver, CustRollOver.TurnOver
	INTO @RollOver, @TurnOver FROM
		CustRollOver
	WHERE
		CustRollOver.CustId = CustId
			AND CustRollOver.RollOverType = 2
			AND CustRollOver.BonusCode = BonusCode
            AND CustRollOver.`Status` = 1  FOR UPDATE;
            
	SELECT 
		@RollOver AS RollOver,
		@TurnOver AS TurnOver;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Common_RollOver_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Common_RollOver_Set`(
	IN CustId INT,
    IN UserName VARCHAR(30),
    IN SiteId INT,
    #IN RollOverType TINYINT,   
    IN BonusCode VARCHAR(50),
	IN RollOver DECIMAL(18,2),
	IN Memo VARCHAR(200),
    IN RefTransId VARCHAR(45),
    OUT AfterRollOver DECIMAL(18,4)
)
BEGIN
	# 20200204@Scott (MemberSite, Service, Backend): Set RollOver
    
	SET AfterRollOver = RollOver;
    
	SELECT 
		RollOver + CustRollOver.RollOver
	INTO AfterRollOver FROM
		CustRollOver
	WHERE
		CustRollOver.CustId = CustId
			#AND CustRollOver.RollOverType = RollOverType
			AND CustRollOver.BonusCode = BonusCode
	FOR UPDATE;
	
	SET @MainType = 2;
	
	IF RollOver > 0 THEN
		SET @MainType = 1;
	END IF;
	
	INSERT INTO RollOverLog(CustId,UserName,SiteId,RollOverType,MainType, ChangeRollOver, BeforeRollOver ,AfterRollOver, Memo, RefTransId) VALUES
	(CustId, UserName, SiteId,2, @MainType, ABS(RollOver), AfterRollOver - RollOver, AfterRollOver, Memo, RefTransId);
	
	INSERT INTO CustRollOver(CustId, RollOverType, BonusCode, SiteId, RollOver) VALUE(CustId, 2, BonusCode, SiteId, RollOver) 
	ON DUPLICATE KEY 
	UPDATE CustRollOver.RollOver=AfterRollOver;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Common_TpCustInfoByTpUserName_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Common_TpCustInfoByTpUserName_Get`(
	IN TpId INT,
	IN TpUserName VARCHAR(50)
)
BEGIN
		# 20200210@Scott (Service): Get Customer By TpUserName
        
		SELECT 
			TC.CustId,
            C.CurrencyId,
            TC.UserName,
            TC.SiteId
		FROM
			Customer AS C
				INNER JOIN
			TpCustInfo AS TC ON C.CustId = TC.CustId AND TC.TpId=TpId
		WHERE TC.TpUserName=TpUserName AND TC.TpId=TpId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Common_TpList_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Common_TpList_Get`(
)
BEGIN
   	# 20200210@Scott Wang (Service): Get List of Tp
    
	SELECT 
		`TpList`.`Id`,
		`TpList`.`TpName`,
		`TpList`.`Setting`,
		`TpList`.`Status`,
        TpList.TicketKey
	FROM
		`TpList`;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Common_WinLostAmount_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Common_WinLostAmount_Set`(
	IN TpId INT,
	IN CustId INT,
    IN UserName VARCHAR(30),
    IN SiteId INT,
    IN Winnings DECIMAL(18,2),
	IN Losses DECIMAL(18,2),    
	IN Memo VARCHAR(200),
    IN RefTransId VARCHAR(45)
)
BEGIN
	# 20200215@Scott (MemberSite, Service, Backend): Set WinLostAmount
    
    SET @Winnings = 0;
    SET @Losses = 0;
    
	SELECT 
		CustWinLost.Winnings,
		CustWinLost.Losses
	INTO @Winnings, @Losses FROM
		CustWinLost
	WHERE
		CustWinLost.CustId = CustId
	FOR UPDATE;
    
	INSERT INTO CustWinLost(CustId, SiteId, Winnings, Losses) VALUE(CustId, SiteId, Winnings, Losses) 
	ON DUPLICATE KEY 
	UPDATE CustWinLost.Winnings = CustWinLost.Winnings + Winnings, CustWinLost.Losses = CustWinLost.Losses + Losses;
    
    IF Winnings > 0 THEN
		SET @MainType = 2;
		
		IF Winnings > 0 THEN
			SET @MainType = 1;
		END IF;   
        
		INSERT INTO WinningsLog
		(CustId, UserName, SiteId, TpId, MainType, ChangeWinnings, BeforeWinnings, AfterWinnings, Memo, RefTransId)
		VALUES
		(CustId, UserName, SiteId, TpId, @MainType, Winnings, @Winnings, @Winnings + Winnings, Memo, RefTransId);         
    END IF;
    
    IF Losses > 0 THEN
		SET @MainType = 2;
		
		IF Losses > 0 THEN
			SET @MainType = 1;
		END IF;       
    
		INSERT INTO LossesLog
		(CustId, UserName, SiteId, TpId, MainType, ChangeLosses, BeforeLosses, AfterLosses, Memo, RefTransId)
		VALUES
		(CustId, UserName, SiteId, TpId, @MainType, Losses, @Losses, @Losses + Losses, Memo, RefTransId);         
    END IF;    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Common_WithdrawalAvailableBalance_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Common_WithdrawalAvailableBalance_Get`(
	IN CustId INT
    #IN SiteId INT
)
BEGIN
	# 20200214@Scott (MemberSite): Get WithdrawalAvailableBalance
    # 20200217@Scott (MemberSite): Remove Calculate WithdrawlSum
    
    SET @Balance = 0;
    SET @WithdrawlSum = 0;
    SET @WithdrawalLimit = 0;
    SET @TurnOver = 0;    
    
	SELECT 
		CustWallet.Balance
	INTO @Balance FROM
		CustWallet
	WHERE
		CustWallet.CustId = CustId
			#AND CustWallet.SiteId = SiteId
	FOR UPDATE;

	/*SELECT 
		CustTransSummary.WithdrawlSum
	INTO @WithdrawlSum FROM
		CustTransSummary
	WHERE
		CustTransSummary.CustId = CustId
			#AND CustTransSummary.Site = SiteId
	FOR UPDATE;*/
    
	SELECT 
		CustWithdrawalLimit.WithdrawalLimit,
		CustWithdrawalLimit.TurnOver
	INTO @WithdrawalLimit , @TurnOver FROM
		CustWithdrawalLimit
	WHERE
		CustWithdrawalLimit.CustId = CustId
			#AND CustWithdrawalLimit.SiteId = SiteId
	FOR UPDATE;

	SET @WithdrawalLimit = (@WithdrawalLimit - @TurnOver/* + @WithdrawlSum*/);
    
	SELECT @Balance - @WithdrawalLimit AS WithdrawalAvailableBalance;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Common_WithdrawalLimitTurnOver_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Common_WithdrawalLimitTurnOver_Set`(
	IN TpId INT,
	IN CustId INT,
    IN UserName VARCHAR(30),
    IN SiteId INT,
	IN TurnOver DECIMAL(18,2),
	IN Memo VARCHAR(200),
    IN RefTransId VARCHAR(45)    
)
BEGIN
	# 20200214@Scott (MemberSite, Service, Backend): Set TurnOver of WithdrawalLimit

	SET @TurnOver = 0;    
   
    SELECT 
		CustWithdrawalLimit.TurnOver
	INTO @TurnOver FROM
		CustWithdrawalLimit
	WHERE
		CustWithdrawalLimit.CustId = CustId
	FOR UPDATE;

	INSERT INTO CustWithdrawalLimit(CustId, SiteId, TurnOver) VALUE(CustId, SiteId, TurnOver) 
	ON DUPLICATE KEY 
	UPDATE CustWithdrawalLimit.TurnOver = CustWithdrawalLimit.TurnOver + TurnOver;
	
	SET @MainType = 2;
	
	IF TurnOver > 0 THEN
		SET @MainType = 1;
	END IF;    
    
	INSERT INTO TurnOverLog
	(TurnOverType, CustId, UserName, SiteId, TpId, MainType, ChangeTurnOver, BeforeTurnOver, AfterTurnOver, Memo, RefTransId)
	VALUES
	(1, CustId, UserName, SiteId, TpId, @MainType, TurnOver, @TurnOver, @TurnOver + TurnOver, Memo, RefTransId);        
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Common_WithdrawalLimit_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Common_WithdrawalLimit_Set`(
	IN CustId INT,
    IN UserName VARCHAR(30),
    IN SiteId INT,
	IN WithdrawalLimit DECIMAL(18, 2),
	IN Memo VARCHAR(200),
    IN RefTransId VARCHAR(45),
    OUT AfterWithdrawalLimit DECIMAL(18, 4)
)
BEGIN
	# 20200214@Scott (MemberSite, Service, Backend): Set WithdrawalLimit
    IF WithdrawalLimit <> 0 THEN 
		SET AfterWithdrawalLimit = WithdrawalLimit;
		SELECT CustWithdrawalLimit.WithdrawalLimit + WithdrawalLimit INTO AfterWithdrawalLimit FROM CustWithdrawalLimit WHERE CustWithdrawalLimit.CustId=CustId FOR UPDATE;
		
		SET @MainType = 2;
		
		IF WithdrawalLimit > 0 THEN
			SET @MainType = 1;
		END IF;
		
		INSERT INTO WithdrawalLimitLog(CustId,UserName,SiteId, MainType, ChangeWithdrawalLimit, BeforeWithdrawalLimit, AfterWithdrawalLimit, Memo, RefTransId) VALUES
		(CustId, UserName, SiteId, @MainType, ABS(WithdrawalLimit), AfterWithdrawalLimit - WithdrawalLimit, AfterWithdrawalLimit, Memo, RefTransId);
		
		INSERT INTO CustWithdrawalLimit(CustId, SiteId, WithdrawalLimit) VALUE(CustId, SiteId, AfterWithdrawalLimit) 
		ON DUPLICATE KEY 
		UPDATE CustWithdrawalLimit.WithdrawalLimit=AfterWithdrawalLimit;
    END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `CustBankInfo_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `CustBankInfo_Get`(
	IN CustId INT,
    IN SiteId INT
)
BEGIN
	# 20191128@Scott (MemBetSite): to Get CustBankInfo.
	SELECT 
		`CustBankInfo`.`AccountName`, `CustBankInfo`.`BankInfo`
	FROM
		`Siebog`.`CustBankInfo`
	WHERE
		CustBankInfo.CustId = CustId
			AND CustBankInfo.SiteId = SiteId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `CustBankInfo_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `CustBankInfo_Set`(
	IN CustId INT,
    IN SiteId INT,
    IN AccountName VARCHAR(50),
    IN BankInfo JSON
)
BEGIN
	# 20191128@Scott (MemBetSite): to Set CustBankInfo.
	INSERT INTO CustBankInfo(CustId, SiteId, AccountName, BankInfo) VALUE(CustId, SiteId, AccountName, BankInfo) 
	ON DUPLICATE KEY 
	UPDATE CustBankInfo.BankInfo=BankInfo;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `CustInfoLog1_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `CustInfoLog1_Get`(
	IN CustId INT,
    IN SiteId INT
)
BEGIN
	# 20191222@Scott (MemBetSite): to Get CustInfo Log 1.
    SET @LastLoginTime = NULL;
    
	SELECT 
		LastLoginTime
	INTO 
		@LastLoginTime
	FROM
		Customer
	WHERE
		Customer.CustId = CustId
			AND Customer.SiteId = SiteId;

	SET @LastTransaction= NULL;
    
	SELECT 
		CreateTime
	INTO 
		@LastTransaction
	FROM
		FinancialLog
	WHERE
		FinancialLog.CustId = CustId
			AND FinancialLog.SiteId = SiteId
	ORDER BY CreateTime DESC LIMIT 1;
            
	SELECT DATE_FORMAT(@LastLoginTime, '%Y-%m-%dT%TZ')  AS LastLoginTime, DATE_FORMAT(@LastTransaction, '%Y-%m-%dT%TZ') AS LastTransaction;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `CustInfo_Ins` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `CustInfo_Ins`(
    IN SiteId INT,
    IN UserName VARCHAR(30),
    IN Userpwd VARCHAR(50),
    IN CurrencyId TINYINT,
    IN UserLang VARCHAR(4),
    IN AgentId INT,
    IN AllAgentId JSON,
    IN UserLevel TINYINT,
    OUT CustId INT
)
BEGIN
	# 20191117@Scott (MemberSite): add Customer
	INSERT INTO Customer(SiteId,UserName,Userpwd,CurrencyId,UserLang,ActStatus,AgentId,AllAgentId,UserLevel,`Status`)
	SELECT 
		SiteId,
		UserName,
		Userpwd,
		CurrencyId,
		UserLang,
		1,
		AgentId,
		AllAgentId,
		UserLevel,
		1
	FROM DUAL WHERE
		NOT EXISTS( SELECT 
				1
			FROM
				Customer
			WHERE
				Customer.UserName = UserName
					AND Customer.SiteId = SiteId);
                    
	SET CustId = LAST_INSERT_ID();
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Customer_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Customer_Get`(
	IN CustId INT
)
BEGIN
		# 20191117@Scott (MemberSite): Get Customer
		SELECT 
		C.`CustId`,
		C.`SiteId`,
		C.`UserName`,
		C.`Userpwd`,
		C.`CurrencyId`,
		C.`UserLang`,
		C.`ActStatus`,
		C.`AgentId`,
		C.`AllAgentId`,
		C.`UserLevel`,
		C.`IsTest`,
        C.`Status`
	FROM
		Customer AS C
	WHERE
		C.`CustId` = CustId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `DepositCallBack_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `DepositCallBack_Get`(
	IN DepositId INT
)
BEGIN
   	# 20191226@Scott(Payment Service): Get DepositInfo by DepositId
	SELECT 
		DI.DepositId,
		DI.TransId,
		DI.DepositType,
		DI.CustId,
		DI.UserName,
		DI.SiteId,
		DI.DepositInfo,
		DI.Amount,
		DI.`Status`
	FROM
		DepositInfo AS DI
	WHERE
		DI.DepositId = DepositId
			AND DI.DepositType IN (1 , 2); #Protect Type only for Online Payment
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `DepositInfoCancel_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `DepositInfoCancel_Set`(
	IN DepositId INT,
    IN SiteId INT,
    IN CustId INT
)
BEGIN
	# 20200102@Scott (MemberSite): DepositInfo to be Cancel By Customer.
    
    SET @CustId = 0;
    
	SELECT 
		DepositInfo.CustId
	INTO @CustId FROM
		DepositInfo
	WHERE
		DepositInfo.DepositId = DepositId
			AND DepositInfo.`Status` = 1
			AND DepositInfo.SiteId = SiteId
            AND DepositInfo.CustId = CustId
	FOR UPDATE NOWAIT;
    
    IF @CustId > 0 THEN
		UPDATE DepositInfo 
		SET 
			DepositInfo.`Status` = 5
		WHERE
			DepositInfo.DepositId = DepositId
				AND DepositInfo.`Status` = 1
				AND DepositInfo.SiteId = SiteId
				AND DepositInfo.CustId = @CustId;
    END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `DepositInfoFirstDeposit_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `DepositInfoFirstDeposit_Set`(
	IN DepositId INT
)
BEGIN
	# 20191126@Scott (BackendSite): DepositInfo set FirstDeposit flag
	UPDATE `Siebog`.`DepositInfo` 
	SET 
		DepositInfo.FirstDeposit = true
	WHERE
		DepositInfo.DepositId = DepositId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `DepositInfoRecent_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `DepositInfoRecent_Get`(
	IN CustId INT,
    IN SiteId INT,
    IN TopNum INT
)
BEGIN
	# 20191215@Scott(MemberSite): Get List of DepositInfo for Recent Transcation
    # 20200110@Scott(MemberSite): Add Fields with RefId, DepositTime, PicFileName
    
	SELECT 
		DATE_FORMAT(DI.CreateTime, '%Y-%m-%dT%TZ') AS CreateTime,
		DI.DepositType,
		DI.TransId,
		DI.Amount,
        #DI.postScript,
		DI.`Status`,
        DI.DepositInfo,
        DI.RefId,
        DATE_FORMAT(DI.DepositTime, '%Y-%m-%dT%TZ') AS DepositTime,
        DR.PicFileName
	FROM
		DepositInfo AS DI LEFT OUTER JOIN
        DepositReceipt AS DR ON DI.DepositId = DR.DepositId
	WHERE
		DI.CustId = CustId
			AND DI.SiteId = SiteId
	ORDER BY DI.DepositId DESC
	LIMIT TopNum;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `DepositInfoSearch_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `DepositInfoSearch_Get`(
	#IN TransId VARCHAR(15),
    IN CustId INT,    
    IN StartDate BIGINT,
    IN EndDate BIGINT,
    IN SiteId INT,
	IN PageSize INT,
    INOUT PageNumber INT,
    OUT TotalPages INT,
    OUT RowCount INT,
    OUT StartIndex INT
)
BEGIN
	# 20191219@Scott (MemberSite): Get DepositInfo for History
	SET PageSize = ABS(PageSize);
    SET PageNumber = ABS(PageNumber);
    SET RowCount=0;
    #FROM_UNIXTIME
    SET @StartDate = FROM_UNIXTIME(StartDate);
    SET @EndDate = FROM_UNIXTIME(EndDate);
    IF StartDate IS NOT NULL AND EndDate IS NOT NULL THEN
		SELECT 
			COUNT(1)
		INTO RowCount FROM
			DepositInfo
		WHERE
			(DepositInfo.CreateTime BETWEEN @StartDate AND @EndDate)
				AND DepositInfo.SiteId = SiteId AND DepositInfo.CustId=CustId;
    
    	IF RowCount = 0 THEN 
			SET PageNumber = 1;
			SET TotalPages = 0;
		ELSE
			SET TotalPages = CEIL(RowCount/PageSize);		
			IF PageNumber > TotalPages THEN 
				SET PageNumber = TotalPages;
			END IF;
			
			SET StartIndex = (PageNumber - 1) * PageSize; 
            
			SELECT 
                DATE_FORMAT(DI.CreateTime, '%Y-%m-%dT%TZ') AS CreateTime,            
                DI.DepositType,                 
				DI.TransId, 
                DI.DepositInfo, 
                DI.Amount, 
                #DI.ActualDeposit, 
				#DI.postScript,                
                DI.`Status`,
				DI.RefId,
				DATE_FORMAT(DI.DepositTime, '%Y-%m-%dT%TZ') AS DepositTime,
				DR.PicFileName                
			FROM
				DepositInfo AS DI LEFT OUTER JOIN
				DepositReceipt AS DR ON DI.DepositId = DR.DepositId
			WHERE
				(DI.CreateTime BETWEEN @StartDate AND @EndDate)
					AND DI.SiteId = SiteId AND DI.CustId=CustId
                    ORDER BY DI.DepositId DESC
                    LIMIT StartIndex , PageSize;
		END IF;
	END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `DepositInfo_Ins` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `DepositInfo_Ins`(
	IN DepositType INT, # OnlineBanking = 1, MobilePayment = 2, WireTransfer = 3,
    IN CustId INT,
    IN UserName VARCHAR(30),
    IN CurrencyId TINYINT,
    IN SiteId INT ,
    IN DepositInfo JSON,
    IN Amount DECIMAL(18,4),
    IN Memo VARCHAR(100),
    IN Prefix CHAR(2),
    OUT vPostScript VARCHAR(45),
    OUT vTransId VARCHAR(30)
    )
BEGIN
	# 20191125@Scott (MemberSite): DepositInfo Request
    # 20191212@Scott (MemberSite): Removed Fill 0 for TransId.
    
	SET @DepositId = 0;
    SET vPostScript = NULL;
    
    IF DepositType = 3 THEN #WireTransfer
		SET vPostScript= Siebog.Nextval('Deposit');
        if RIGHT(Prefix,1) <> 'P' THEN
			SET vPostScript= CONCAT(RIGHT(Prefix,1),vPostScript);
        END IF;
    END IF;

	INSERT INTO DepositInfo(DepositType,CustId,UserName,CurrencyId,SiteId,DepositInfo,Amount,Memo,PostScript,ActualDeposit) 
	VALUES(DepositType,CustId,UserName,CurrencyId,SiteId,DepositInfo,Amount,Memo,vPostScript,Amount);
    
	SET @DepositId=LAST_INSERT_ID();
	SET @TransId=CONCAT(Prefix, @DepositId);
	UPDATE DepositInfo 
	SET 
		DepositInfo.TransId = @TransId
	WHERE
		DepositInfo.DepositId = @DepositId;
        
	SET vTransId = @TransId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `DepositInfo_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `DepositInfo_Set`(
	IN DepositId INT,
    IN SiteId INT,
    IN ActualDeposit decimal(18, 2),
    IN `Comment` VARCHAR(500),
    IN ForStatus JSON,
    IN `Status` TINYINT,
    IN ThirdId  VARCHAR(100),
    OUT CustId INT,
    OUT UserName VARCHAR(30)
)
BEGIN
	# 20191126@Scott (BackendSite, Payment Service): DepositInfo to do Approved, Online Backing
    # 20191127@Scott (BackendSite): Error Proof for ActualDeposit
	# 20200108@Scott (BackendSite): Remove Updated for ActualDeposit
    # 20200113@Scott (BackendSite): Rejected cancel with AdjustFlag = 1
    
    SET @DepositId=0;

	SELECT 
		DepositInfo.CustId, DepositInfo.UserName, DepositInfo.DepositId
	INTO CustId , UserName, @DepositId FROM
		DepositInfo
	WHERE
		DepositInfo.DepositId = DepositId
			AND JSON_CONTAINS(ForStatus , CONVERT(DepositInfo.`Status`,CHAR))=1
			AND DepositInfo.SiteId = SiteId
	FOR UPDATE NOWAIT;
    
    IF @DepositId<>0 THEN
		UPDATE `Siebog`.`DepositInfo` 
		SET 
			/*DepositInfo.ActualDeposit = IF(`Status` = 1,
				ActualDeposit,
				DepositInfo.ActualDeposit),*/
			DepositInfo.`Status` = IF(`Status` = 0,
				`DepositInfo`.`Status`,
				`Status`),
			DepositInfo.`Comment` = `Comment`,
			DepositInfo.RefId = ThirdId,
			DepositInfo.CallbackTime = IF(ThirdId IS NULL,
				DepositInfo.CallbackTime,
				NOW())
		WHERE
			DepositInfo.DepositId = @DepositId;
	END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `DepositList_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `DepositList_Get`(
	IN SiteId INT,
    IN DepositType INT,
    IN StartDate BIGINT,
    IN EndDate BIGINT,    
    IN DepositId INT,
	IN TransId VARCHAR(30),
    IN UserName VARCHAR(30),
    IN CurrencyId TINYINT,
    IN `Status` INT,
	IN PageSize INT,
    INOUT PageNumber INT,
    OUT TotalPages INT,
    OUT RowCount INT,
    OUT StartIndex INT  
)
BEGIN
   	# 20191125@Eddie Wu (BackendSite): Get List of DepositInfo
    # 20200120@Scott (BackendSite): Add Col for Calvin Code
    
	SET @StartDate = FROM_UNIXTIME(StartDate);
    SET @EndDate = FROM_UNIXTIME(EndDate);
    
	SELECT 
		COUNT(1)
	INTO RowCount FROM
		DepositInfo AS DI
	WHERE
		DI.SiteId = SiteId
			AND DI.DepositType = IFNULL(DepositType, DI.DepositType)
			AND DI.CreateTime BETWEEN @StartDate AND @EndDate
			AND (DI.TransId = IFNULL(TransId, DI.TransId)
			OR DI.DepositId = IFNULL(DepositId, DI.DepositId))
			AND DI.UserName = IFNULL(UserName, DI.UserName)
            AND DI.CurrencyId = IFNULL(CurrencyId, DI.CurrencyId)
			AND DI.`Status` = IFNULL(`Status`, DI.`Status`);    
            
	IF RowCount = 0 THEN 
		SET PageNumber = 1;
		SET TotalPages = 0;
	ELSE
		SET TotalPages = CEIL(RowCount/PageSize);		
		IF PageNumber > TotalPages THEN 
			SET PageNumber = TotalPages;
		END IF;
		
		SET StartIndex = (PageNumber - 1) * PageSize; 
			
		SELECT 
			DI.DepositId,
			DI.TransId,
			DI.DepositType,
            DI.CurrencyId,
			DI.CustId,
			DI.UserName,
			DI.SiteId,
			DI.DepositInfo,
			DI.Amount,
			DI.ActualDeposit,
			DATE_FORMAT(DI.CreateTime, '%Y-%m-%dT%TZ') AS CreateTime,
			DATE_FORMAT(DI.TPCreateTime, '%Y-%m-%dT%TZ') AS TPCreateTime,
			DATE_FORMAT(DI.CallbackTime, '%Y-%m-%dT%TZ') AS CallbackTime,
			DI.FirstDeposit,
			DI.ThirdId,
			DI.Memo,
			#DI.PostScript,
			DI.`Status`,
			DI.DepositInfo->>'$.AccountName' AS AccountName,            
			DI.DepositInfo->>'$.AccountNo' AS AccountNo,
 			IF(DR.DepositId IS NULL, 0, 1) AS Receipt,
            SI.SiteName, 
            BIS.BankCode
		FROM
			DepositInfo AS DI LEFT OUTER JOIN
			DepositReceipt AS DR ON DI.DepositId = DR.DepositId
			INNER JOIN 
			BankInfoSys AS BIS ON DI.DepositInfo->'$.SysId'= BIS.Id
			INNER JOIN 
			ServerInfo AS SI ON SI.SiteId = DI.SiteId
		WHERE
			#DI.SiteId = SiteId AND
                DI.DepositType = IFNULL(DepositType, DI.DepositType)
				AND DI.CreateTime BETWEEN @StartDate AND @EndDate
				AND (DI.TransId = IFNULL(TransId, DI.TransId)
				OR DI.DepositId = IFNULL(DepositId, DI.DepositId))
				AND DI.UserName = IFNULL(UserName, DI.UserName)
				AND DI.CurrencyId = IFNULL(CurrencyId, DI.CurrencyId)
				AND DI.`Status` = IFNULL(`Status`, DI.`Status`)
			ORDER BY DI.CreateTime
			LIMIT StartIndex , PageSize;
		END IF;                
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `DepositLog_Ins` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `DepositLog_Ins`(
	IN DepositId INT,
    IN SiteId INT,
    IN `Action` INT,
    IN `Comment` VARCHAR(500),
    IN ActualDeposit decimal(18,8),
    IN Operator VARCHAR(30)
)
BEGIN
	# 20191126@Scott (BackendSite, Payment Service): to Record DepositLog.
	INSERT INTO `Siebog`.`DepositLog`
		(`DepositId`,
		`SiteId`,
		`Action`,
		`Comment`,
        ActualDeposit,
        Operator
        )
	VALUES 
		(DepositId,
		SiteId,
		`Action`,
		`Comment`,
        ActualDeposit,
        Operator
		);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `DepositLog_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `DepositLog_Set`(
	IN DepositId INT,
    IN SiteId INT,
    IN `Action` INT,
    IN `Comment` VARCHAR(500),
    IN ActualDeposit decimal(18,8)
)
BEGIN
	# 20191126@Scott (BackendSite): to Record DepositLog.
	INSERT INTO `Siebog`.`DepositLog`
		(`DepositId`,
		`SiteId`,
		`Action`,
		`Comment`,
        ActualDeposit
        )
	VALUES 
		(DepositId,
		SiteId,
		`Action`,
		`Comment`,
        ActualDeposit
		);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `DepositReceiptPic_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `DepositReceiptPic_Set`(
	IN CustId INT,
    IN DepositId INT,
    IN PicFileName VARCHAR(100),
    IN DepositPic MEDIUMBLOB
)
BEGIN
	# 20191230@Scott (MemberSite): Upload Picture of Deposit Receipt

	SET @DepositId = 0;
	SELECT 
		DepositInfo.DepositId
	INTO @DepositId FROM
		DepositInfo
	WHERE
		DepositInfo.CustId = CustId
			AND DepositInfo.DepositId = DepositId
			AND DepositInfo.`Status` IN (1 , 7);
            
	IF @DepositId > 0 THEN
		INSERT INTO DepositReceipt(DepositId, PicFileName, DepositPic) VALUE(@DepositId, PicFileName, DepositPic) 
		ON DUPLICATE KEY 
		UPDATE DepositReceipt.DepositPic = DepositPic, DepositReceipt.PicFileName = PicFileName;
    END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `DepositReceipt_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `DepositReceipt_Get`(
	IN DepositId INT,
    IN SiteId INT
)
BEGIN
	# 20191231@Scott (BackendSite): Get Receipt of Deposit
    # 20200110@Scott (MemberSite): Removed RefNo & DepositTime

	SELECT 
		DI.DepositId,
        /*DR.RefNo,
        DATE_FORMAT(DR.DepositTime, '%Y-%m-%dT%TZ') AS DepositTime,*/
        DR.PicFileName,
        DR.DepositPic
	FROM
		DepositReceipt AS DR
			INNER JOIN
		DepositInfo AS DI ON DI.DepositId = DR.DepositId
	WHERE DI.DepositId=DepositId AND DI.SiteId=SiteId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `DepositReceipt_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `DepositReceipt_Set`(
	IN CustId INT,
    IN DepositId INT,
    IN RefNo VARCHAR(100),
	IN DepositTime INT
)
BEGIN
	# 20191230@Scott (MemberSite): Deposit Receipt Set
	# 20200107@Scott (MemberSite): Adjust Receipt data to DepositInfo
	SET @DepositId = 0;
	SET @DepositTime = FROM_UNIXTIME(DepositTime);
    
	UPDATE DepositInfo 
	SET 
		DepositInfo.RefId = RefNo,
		DepositInfo.DepositTime = @DepositTime,
        DepositInfo.`Status` = 1
	WHERE
		DepositInfo.CustId = CustId
			AND DepositInfo.DepositId = DepositId
            AND DepositInfo.DepositType = 3
			AND DepositInfo.`Status` IN (1 , 7);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `DepositRequestComment_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `DepositRequestComment_Get`(
	IN DepositId INT,
    IN SiteId INT
)
BEGIN
	# 20191125@Eddie Wu (BackendSite): Get Comment of DepositInfo
    
	SELECT 
		DATE_FORMAT(CreateTime, '%Y-%m-%dT%TZ') AS CreateTime,
        `Comment`,
        `Action`, 
        ActualDeposit,
        Operator
	FROM
		DepositLog
	WHERE
		DepositLog.DepositId = DepositId
			AND DepositLog.SiteId = SiteId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `DepositRequestTransID_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `DepositRequestTransID_Get`(
	IN DepositId INT,
    IN SiteId INT
)
BEGIN
   	# 20191125@Eddie Wu (BackendSite): Get DepositInfo by DepositId
    # 20200113@Scott (BackendSite): Added AdjustFlag
    # 20200127@Scott (BackendSite): Added Real Name(Account Name)
    
	SELECT DI.DepositId,
		DI.TransId,
		DI.DepositType,
		DI.CustId,
		DI.UserName,
        CBI.AccountName AS RealName,
		CP.Email,        
        DI.CurrencyId,
		DI.SiteId,
        SI.SiteName,
		DI.DepositInfo,
		DI.Amount,
		DI.ActualDeposit,
        DI.`Comment`,
		DATE_FORMAT(DI.CreateTime, '%Y-%m-%dT%TZ') AS CreateTime,
		DATE_FORMAT(DI.TPCreateTime, '%Y-%m-%dT%TZ') AS TPCreateTime,
		DATE_FORMAT(DI.CallbackTime, '%Y-%m-%dT%TZ') AS CallbackTime,
		DI.FirstDeposit,
		DI.Memo,
		#DI.PostScript,
        DI.RefId,
        DATE_FORMAT(IF(DI.DepositType=3, DI.DepositTime, DI.CallbackTime), '%Y-%m-%dT%TZ') AS DepositTime,
		DI.`Status`,
        BIS.BankName,
        BIS.BankCode,        
		DI.DepositInfo->>'$.AccountName' AS AccountName,            
		DI.DepositInfo->>'$.AccountNo' AS AccountNo,        
        PL.PayName,
        IF(DR.DepositPic IS NULL, 0, 1) AS Receipt,
        DR.PicFileName
	FROM
		DepositInfo AS DI LEFT OUTER JOIN
		DepositReceipt AS DR ON DI.DepositId = DR.DepositId
		INNER JOIN 
        BankInfoSys AS BIS ON DI.DepositInfo->'$.SysId'= BIS.Id        
        INNER JOIN 
		ServerInfo AS SI ON SI.SiteId = DI.SiteId
        LEFT OUTER JOIN 
		PayList AS PL ON DI.DepositInfo->'$.PayId'= PL.Id
        LEFT OUTER JOIN 
        CustProfile AS CP ON CP.CustId = DI.CustId
        LEFT OUTER JOIN 
        CustBankInfo AS CBI ON DI.CustId = CBI.CustId              
	WHERE
		DI.DepositId = DepositId;
			#AND DI.SiteId = SiteId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `LangId_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `LangId_Set`(
	IN CustId INT,
	IN LangId VARCHAR(4)
)
BEGIN
	UPDATE 
		`Siebog`.`Customer`
	SET
		`Customer`.`UserLang` = LangId
	WHERE `Customer`.`CustId` = CustId AND `Customer`.`UserLang` <> LangId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `LoaclBankList_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `LoaclBankList_Get`(
	IN SiteId INT
)
BEGIN
	# 20191128@Eddie(BackendSite): Get List of BankAccountOff
	SELECT 
		BDO.Id AS BankId,
		BDO.BankAccountName AS AccountName,
		BDO.BankAccountNo AS AccountNo,
		BDO.SysId,
		BDO.MinAmount,
		BDO.MaxAmount,
		BDO.DailyLimitAmount,
		BDO.Purpose,
		BDO.PointLevel,
		BDO.Memo,
		BDO.Sort AS BSort,
		BIS.CurrencyId,
		BIS.BankCode,
		BIS.BankName,
		BIS.BankLogo,
		BIS.BankSite,
		BIS.Sort,
		BDO.`Status`,
        IFNULL(BOD.Amount, 0) AS Dummy
	FROM
		BankDetailOff BDO
			INNER JOIN
		BankInfoSys BIS ON BDO.SysId = BIS.Id
			LEFT OUTER JOIN
        BankOffDummy BOD ON BOD.SysId = BIS.Id AND BOD.AccountNo=BDO.BankAccountNo
	WHERE
		BDO.SiteId = SiteId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `LoginAdminUser_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `LoginAdminUser_Get`(
	IN UserName varchar(30),
    IN SiteId INT
)
BEGIN
	# 20191205@Scott(BackendSite): Get Login AdminUser
	SELECT 
		`AdminUser`.`UserId`,
		`AdminUser`.`SiteId`,
		`AdminUser`.`UserName`,
		`AdminUser`.`Userpwd`,
		`AdminUser`.IsSuper,
        `AdminUser`.`Status`,
        `AdminUser`.`Role`
	FROM
		`Siebog`.`AdminUser`
	WHERE
		`AdminUser`.`UserName` = UserName
			AND `AdminUser`.SiteId = SiteId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `LoginCheckIn_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `LoginCheckIn_Set`(
	IN IsOnline BOOLEAN,
    IN CustId INT,
    IN IP VARCHAR(15),
    IN Token VARCHAR(30)
)
BEGIN
	IF IsOnline THEN
		UPDATE OnUser 
		SET 
			OnUser.IP = IP,
			OnUser.ReloadTime = NOW()
		WHERE
			OnUser.CustId = CustId AND OnUser.Token = Token;
	ELSE
		DELETE FROM OnUser 
		WHERE
			OnUser.CustId = CustId;
	END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `LoginCustInfo_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `LoginCustInfo_Get`(
	IN UserName VARCHAR(30),
    IN SiteId INT
)
BEGIN
	SELECT 
		Customer.CustId,
		Customer.SiteId,
		Customer.UserName,
		Customer.Userpwd,
		Customer.CurrencyId,
		Customer.UserLang,
		Customer.ActStatus,
		Customer.AgentId,
		Customer.AllAgentId,
		Customer.UserLevel,
        Customer.IsTest,
        Customer.`Status`
	FROM
		Customer
	WHERE
		Customer.UserName = UserName
			AND Customer.SiteId = SiteId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `LoginLog_Ins` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `LoginLog_Ins`(
	IN CustId INT,
    IN UserName VARCHAR(30),
    IN SiteId INT ,
    IN LoginIp VARCHAR(15),
    IN FromUrl VARCHAR(100),
    IN UserAgent VARCHAR(255)
)
BEGIN
	INSERT INTO LoginLog(CustId,UserName,SiteId,LoginIp,FromUrl,UserAgent) 
	VALUES(CustId,UserName,SiteId,LoginIp,FromUrl,UserAgent);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `LoginRecord_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `LoginRecord_Set`(
	IN CustId INT, 
    IN SiteId INT, 
    IN IP VARCHAR(15), 
    IN Token VARCHAR(30)
)
BEGIN
	INSERT INTO OnUser(CustId, SiteId, IP, Token) VALUE(CustId, SiteId, IP, Token) 
    ON DUPLICATE KEY 
    UPDATE IP=IP ,Token=Token, ReloadTime=NOW();
    UPDATE Customer SET LastLoginTime=NOW() WHERE Customer.CustId=CustId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Member_BonusInfo_Ins` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Member_BonusInfo_Ins`(
	IN PromotionType INT, # 1. 1st Deposit, 2. Re-Deposit, 3. Rebate, 4. Special
    IN BonusCode VARCHAR(50),    
    IN CustId INT,
    IN UserName VARCHAR(30),
    IN CurrencyId TINYINT,
    IN SiteId INT ,
    IN BonusInfo JSON,
    IN LockedAmount DECIMAL(18,4),
    IN BonusAmount DECIMAL(18,4),
    IN ExpiryDays smallint,
    IN Memo VARCHAR(100),
    IN Prefix CHAR(2),
    OUT TransId VARCHAR(30)
    )
BEGIN
	# 20200205@Scott (MemberSite): BonusInfo Request
    
	SET @BonusId = 0;
	
	INSERT INTO BonusInfo(PromotionType,BonusCode,CustId,UserName,CurrencyId,SiteId,BonusInfo,LockedAmount,BonusAmount, ExpiryTime,Memo) 
	VALUES(PromotionType,BonusCode,CustId,UserName,CurrencyId,SiteId,BonusInfo,LockedAmount,BonusAmount,date_add(NOW(), interval ExpiryDays day),Memo);
    
	SET @BonusId=LAST_INSERT_ID();
	SET @TransId=CONCAT(Prefix, @BonusId);
	UPDATE BonusInfo 
	SET 
		BonusInfo.TransId = @TransId
	WHERE
		BonusInfo.BonusId = @BonusId;
        
	SET TransId = @TransId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Member_CustomerByMail_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Member_CustomerByMail_Get`(
	IN Email VARCHAR(100)
)
BEGIN
		# 20200122@Scott (MemberSite): Get Customer By Mail
        
		SELECT 
			C.CustId
		FROM
			Customer AS C
				INNER JOIN
			CustProfile AS P ON C.CustId = P.CustId
		WHERE P.Email=Email;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Member_CustPhotoId_Int` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Member_CustPhotoId_Int`(
	IN CustId INT,
    IN SiteId INT,
    IN PicFileName VARCHAR(100),
    IN PhotoIdPic MEDIUMBLOB
)
BEGIN
	# 20200115@Scott (MemberSite): Upload PhotoIdPic
    
	INSERT INTO CustPhotoId(CustId, SiteId, PicFileName, PhotoIdPic) 
	SELECT 
		CustId, SiteId, PicFileName, PhotoIdPic
	FROM DUAL WHERE
		NOT EXISTS( SELECT 
				1
			FROM
				CustPhotoId
			WHERE
				CustPhotoId.CustId = CustId);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Member_CustProfile_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Member_CustProfile_Get`(
	IN CustId INT,
    IN SiteId INT
)
BEGIN
	# 20191213@Scott (MemberSute): Get CustInfo Profile
    # 20200115@Scott (MemberSute): Rename for SP be Member_CustProfile_Get
    # 20200115@Scott (MemberSute): Added CountryCode, PhoneNo

	SELECT 
		C.CustId,
		C.UserName,
		C.CurrencyId,
		C.UserLevel,
        C.UserLang,
        C.ActStatus,
		C.IsTest,
		C.`Status`,
        DATE_FORMAT(C.LastLoginTime, '%Y-%m-%dT%TZ') AS LastLoginTime,
        DATE_FORMAT(C.RegTime, '%Y-%m-%dT%TZ') AS RegTime,
        P.Email,
        CBI.AccountName,        
        P.`Profile`->>'$.CountryCode' AS CountryCode,
        P.AreaCode,
        P.PhoneNo
	FROM
		Customer AS C
			LEFT OUTER JOIN
		CustProfile AS P ON C.CustId = P.CustId
			LEFT OUTER JOIN
		CustBankInfo AS CBI ON C.CustId = CBI.CustId        
	WHERE
		C.CustId = CustId AND C.SiteId = SiteId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Member_CustProfile_Ins` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Member_CustProfile_Ins`(
	IN CustId INT,
    IN SiteId INT,
    IN Email VARCHAR(100),
    IN `Profile` JSON
)
BEGIN
	# 20191229@Scott (MemBetSite): to Inst Profile for Registry.
    
	INSERT INTO CustProfile(CustId, SiteId, Email, `Profile`) 
	SELECT 
		CustId, SiteId, Email, `Profile`
	FROM DUAL WHERE
		NOT EXISTS( SELECT 
				1
			FROM
				CustProfile
			WHERE
				CustProfile.Email = Email
					/*AND CustProfile.SiteId = SiteId*/);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Member_CustProfile_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Member_CustProfile_Set`(
	IN CustId INT,
    IN SiteId INT,
    IN Email VARCHAR(100),
    IN AreaCode VARCHAR(10),
    IN PhoneNo VARCHAR(50),
    IN `Profile` JSON
)
BEGIN
	# 20191127@Scott (MemBetSite): to Set Profile.
	INSERT INTO CustProfile(CustId, SiteId, Email, AreaCode, PhoneNo, `Profile`) VALUE(CustId, SiteId, Email, AreaCode, PhoneNo, `Profile`) 
	ON DUPLICATE KEY 
	UPDATE CustProfile.`Profile`=`Profile`, CustProfile.PhoneNo = IFNULL(CustProfile.PhoneNo, PhoneNo), CustProfile.AreaCode = IFNULL(CustProfile.AreaCode, AreaCode);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Member_ExistsEmail_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Member_ExistsEmail_Get`(	
	IN CustId INT,
	IN Email VARCHAR(100),
    IN SiteId INT
)
BEGIN
	# 20191127@Scott (MemBetSite): to Checked Email is Exists.
	SELECT 
		COUNT(1)
	FROM
		CustProfile
	WHERE
		CustProfile.Email = Email
			#AND CustProfile.SiteId = SiteId
				AND CustProfile.CustId != CustId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Member_ExistsPhoneNo_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Member_ExistsPhoneNo_Get`(	
	IN CustId INT,
    IN AreaCode VARCHAR(10),
	IN PhoneNo VARCHAR(50),
    IN SiteId INT
)
BEGIN
	# 20191127@Scott (MemBetSite): to Checked PhoneNo is Exists.
	SELECT 
		COUNT(1)
	FROM
		CustProfile
	WHERE
		CustProfile.AreaCode = AreaCode
			AND CustProfile.PhoneNo = PhoneNo
			#AND CustProfile.SiteId = SiteId
				AND CustProfile.CustId != CustId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Member_ExistsUserName_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Member_ExistsUserName_Get`(	
	IN UserName VARCHAR(30),
    IN SiteId INT
)
BEGIN
	# 20191127@Scott (MemBetSite): to Checked UserName is Exists.
    
	SELECT 
		COUNT(1)
	FROM
		Customer
	WHERE
		Customer.UserName = UserName;
			#AND Customer.SiteId = SiteId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Member_PromotionBeenUsed_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Member_PromotionBeenUsed_Set`(
	IN BonusCode VARCHAR(50), 
    IN CurrencyId TINYINT, 
    IN SiteId INT
)
BEGIN
	UPDATE PromotionSpecific 
	SET 
		PromotionSpecific.BeenUsed = PromotionSpecific.BeenUsed + 1
	WHERE
		PromotionSpecific.BonusCode = BonusCode
			AND PromotionSpecific.CurrencyId = CurrencyId
			AND PromotionSpecific.SiteId = SiteId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Member_PromotionCanJoin_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Member_PromotionCanJoin_Get`(
	IN CustId INT,
    IN CurrencyId TINYINT,
    IN SiteId INT,
    IN BonusCode VARCHAR(50),
    OUT CanJoin BOOLEAN,
    OUT Amount DECIMAL(18,4)
)
BEGIN
	SET CanJoin = FALSE;
	SET Amount = 0;
	SET @UnixTime = UNIX_TIMESTAMP();
    SET @PromotionType = 0;
    
	SELECT 
		P.PromotionType
	INTO
		@PromotionType
	FROM
		Promotion P
			INNER JOIN
		PromotionSpecific PS ON P.BonusCode = PS.BonusCode
	WHERE
		PS.SiteId = SiteId
        AND P.`Status`=1
        AND @UnixTime BETWEEN P.PeriodFrom AND P.PeriodTo
        AND (PS.`Usage`=0 OR PS.`Usage` > PS.BeenUsed)
        AND PS.BonusCode = BonusCode
        AND PS.CurrencyId = CurrencyId FOR UPDATE;
        
	IF @PromotionType > 0 THEN
		IF @PromotionType=1 THEN
			SELECT DepositInfo.Amount INTO Amount FROM DepositInfo WHERE DepositInfo.CustId=CustId AND DepositInfo.FirstDeposit=1 LIMIT 1;
			IF Amount > 0
				AND NOT EXISTS(SELECT 1 FROM BonusInfo WHERE BonusInfo.CustId=CustId AND BonusInfo.PromotionType=@PromotionType) 
					AND NOT EXISTS(SELECT 1 FROM WithdrawalInfo WHERE WithdrawalInfo.CustId=CustId AND WithdrawalInfo.`Status` NOT IN (5,7)) THEN
				SET CanJoin = TRUE;
            END IF;
        END IF;
    END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Member_PromotionListByCustId_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Member_PromotionListByCustId_Get`(
    IN SiteId INT,
    IN CustId INT,
    IN CurrencyId INT
)
BEGIN
	# 20200203@Scott (MemberSite): Get Promotion List By CustId
	SET @UnixTime=UNIX_TIMESTAMP();
    
	SELECT 
		P.BonusCode,
		P.PromotionType,
		P.CategoryId,
		P.ExpiryDays,
        P.PeriodTo,
		PS.CurrencyId,
		IFNULL(BI.BonusInfo->> '$.RollOver' , PS.RollOver) AS RollOver,
		IFNULL(BI.BonusInfo->> '$.Bonus' , PS.Bonus) AS Bonus,
		IFNULL(BI.BonusInfo->> '$.BonusType' , PS.BonusType) AS BonusType,
        IFNULL(BI.BonusInfo->> '$.MaxBonus' , PS.MaxBonus) AS MaxBonus,
        P.Sort,
        BI.LockedAmount,
        BI.BonusAmount,
		DATE_FORMAT(BI.ExpiryTime, '%Y-%m-%dT%TZ') AS ExpiryTime,
        BI.`Status` AS BonusInfoStatus
        #BI.BonusInfo->>'$.CountryCode' AS CountryCode,
	FROM
		Promotion P
			INNER JOIN
		PromotionSpecific PS ON P.BonusCode = PS.BonusCode AND PS.CurrencyId = CurrencyId
			LEFT OUTER JOIN
		BonusInfo BI ON P.BonusCode = BI.BonusCode AND BI.CustId = CustId
	WHERE
		(PS.SiteId = SiteId
        AND P.`Status`=1
        AND @UnixTime BETWEEN P.PeriodFrom AND P.PeriodTo
        AND (PS.`Usage`=0 OR PS.`Usage` > PS.BeenUsed)
        ) OR (BI.CustId=CustId/* AND BI.`Status`=1*/);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Member_PromotionList_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Member_PromotionList_Get`(
    IN SiteId INT
)
BEGIN
	# 20200203@Scott (MemberSite): Get Promotion List
	SET @UnixTime=UNIX_TIMESTAMP();
    
	SELECT 
		P.BonusCode,
		P.PromotionType,
		P.CategoryId,
		P.ExpiryDays,
        P.PeriodTo,
		PS.CurrencyId,
		PS.RollOver,
		PS.Bonus,
		PS.BonusType,
        PS.MaxBonus,
        P.Sort
	FROM
		Promotion P
			INNER JOIN
		PromotionSpecific PS ON P.BonusCode = PS.BonusCode
	WHERE
		PS.SiteId = SiteId
        AND P.`Status`=1
        AND @UnixTime BETWEEN P.PeriodFrom AND P.PeriodTo
        AND (PS.`Usage`=0 OR PS.`Usage` > PS.BeenUsed);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Member_Promotion_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Member_Promotion_Get`(
    IN SiteId INT,
    IN BonusCode VARCHAR(50),
    IN CurrencyId INT
)
BEGIN
	# 20200205@Scott (MemberSite): Get Promotion
	SET @UnixTime=UNIX_TIMESTAMP();
    
	SELECT 
		P.BonusCode,
		P.PromotionType,
		P.CategoryId,
		P.ExpiryDays,
        P.PeriodTo,
		PS.CurrencyId,
		PS.RollOver,
		PS.Bonus,
		PS.BonusType,
        PS.MaxBonus
	FROM
		Promotion P
			INNER JOIN
		PromotionSpecific PS ON P.BonusCode = PS.BonusCode
	WHERE
		PS.SiteId = SiteId
        AND P.`Status`=1
        AND @UnixTime BETWEEN P.PeriodFrom AND P.PeriodTo
        AND (PS.`Usage`=0 OR PS.`Usage` > PS.BeenUsed)
        AND PS.BonusCode = BonusCode
        AND PS.CurrencyId = CurrencyId FOR UPDATE;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Member_SubmittedCustPhotoId_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Member_SubmittedCustPhotoId_Get`(
	IN CustId INT,
    IN SiteId INT
)
BEGIN
	# 20191229@Scott (MemBetSite): to Checked Is Submitted PhotoId.
    SET @Submitted = 0;
    
	SELECT 
		1
	INTO @Submitted FROM
		CustPhotoId
	WHERE
		CustPhotoId.CustId = CustId
        AND CustPhotoId.SiteId = SiteId;
        
	SELECT @Submitted;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `MsgContent_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `MsgContent_Get`(
	IN MsgId INT,
    IN CustId INT
)
BEGIN
		UPDATE `Siebog`.`Message` 
		SET 
			Message.Flag = 1
		WHERE
			Message.RecipientCustId = CustId
				AND Message.Id = MsgId;
                
		SELECT 
			`Message`.Content
		FROM
			`Siebog`.`Message`
		WHERE
			Message.RecipientCustId = CustId
				AND Message.`Status` = 1
				AND Message.MainId = 0
                AND Message.Id=MsgId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `MsgCount_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `MsgCount_Get`(
	IN CustId INT,
    IN IsUnread boolean # 0. All, 1.Unread
)
BEGIN
	SELECT 
		COUNT(1)
	FROM
		`Siebog`.`Message`
	WHERE
		Message.RecipientCustId = CustId
			AND Message.Flag = IF(IsUnread = 0, Message.Flag, 0)
			AND Message.`Status` = 1;
			#AND Message.MainId = 0;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `MsgList_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `MsgList_Get`(
	IN CustId INT,
	IN PageSize INT,
    INOUT PageNumber INT,
    OUT TotalPages INT,
    OUT RowCount INT,
    OUT StartIndex INT
)
BEGIN
	SET PageSize = ABS(PageSize);
    SET PageNumber = ABS(PageNumber);
    SET RowCount=0;
    
	SELECT 
		COUNT(1)
	INTO RowCount FROM
		`Siebog`.`Message`
	WHERE
		Message.RecipientCustId = CustId
			AND Message.`Status` = 1
			AND Message.MainId = 0;
        
	IF RowCount = 0 THEN 
		SET PageNumber = 1;
        SET TotalPages = 0;
	ELSE
        SET TotalPages = CEIL(RowCount/PageSize);		
        IF PageNumber > TotalPages THEN 
			SET PageNumber = TotalPages;
        END IF;
        
		SET StartIndex = (PageNumber - 1) * PageSize; 
        
		SELECT 
			`Message`.`Id`,
			`Message`.`Type`,
			`Message`.`SenderCustId`,
			`Message`.`Sender`,
			`Message`.`Title`,
			DATE_FORMAT(`Message`.`CreateTime`, '%Y-%m-%dT%TZ') AS CreateTime,
			`Message`.`Flag`
		FROM
			`Siebog`.`Message`
		WHERE
			Message.RecipientCustId = CustId
				AND Message.`Status` = 1
				AND Message.MainId = 0
		ORDER BY `Message`.`Id` DESC
		LIMIT StartIndex , PageSize;
    END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `MsgRead_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `MsgRead_Set`(
	IN MsgId INT,
	IN CustId INT
)
BEGIN
	UPDATE `Siebog`.`Message` 
	SET 
		Message.Flag = 1
	WHERE
		Message.RecipientCustId = CustId
			AND Message.Id = MsgId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Msg_Del` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Msg_Del`(
	IN MsgId INT,
    IN CustId INT
)
BEGIN
	# 20191204@Scott (MemberSite): Deleted Personal Message Content.
        
	UPDATE `Siebog`.`Message` 
	SET 
		Message.`Status` = 0
	WHERE
		Message.RecipientCustId = CustId
			AND Message.Id = MsgId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Msg_Ins` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Msg_Ins`(
	IN `Type` TINYINT, # 99. System
    IN SiteId INT,
    IN LangId VARCHAR(4),
    IN RecipientCustId INT,
    IN Recipient VARCHAR(30),
	IN SenderCustId INT,
    IN Sender VARCHAR(30),
    IN Title VARCHAR(45),
	IN Content VARCHAR(300),
    IN MainId INT
)
BEGIN
	# 20191204@Scott (MemberSite, Backend): add New Personal Message
    
	INSERT INTO `Siebog`.`Message`
	(`Type`,
	`SiteId`,
	`LangId`,
	`RecipientCustId`,
	`Recipient`,
	`SenderCustId`,
	`Sender`,
	`Title`,
	`Content`,
	`MainId`)
	VALUES
	(`Type`,
	SiteId,
	LangId,
	RecipientCustId,
	Recipient,
	SenderCustId,
	Sender,
	Title,
	Content,
	MainId);

	SELECT LAST_INSERT_ID() AS MsgId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Password_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Password_Set`(
	IN CustId INT,
	IN Md5OldPassword varchar(50),
    IN Md5NewPassword varchar(50),
    OUT Md5Userpwd  varchar(50)
)
BEGIN
	# 20191127@Scott (MemberSite): ChangePassword
	SELECT 
		Customer.Userpwd
	INTO Md5Userpwd FROM
		Customer
	WHERE
		Customer.CustId = CustId
	FOR UPDATE;

	UPDATE Customer 
	SET 
		Customer.Userpwd = Md5NewPassword
	WHERE
		Customer.CustId = CustId
			AND Customer.Userpwd = Md5OldPassword;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `PayCurrency_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `PayCurrency_Get`(
	
)
BEGIN
	SELECT 
		PC.ChannelId,
		PC.CurrencyId,
        PC.PayCurrencyId,
		PC.MinAmount,
		PC.MaxAmount,
		PC.AmountRule,
		PC.AmountShow
	FROM
		PayCurrency AS PC;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `PayDummy_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `PayDummy_Get`(
	IN SiteId INT, 
    IN PayId INT,
    IN CurrencyId int
)
BEGIN
	# 20200106@Scott (Backend, Payment Service): Get Dummy for Online Payment

	SELECT 
		Amount
	FROM
		PayDummy
	WHERE
		PayDummy.SiteId = SiteId
			AND PayDummy.PayId = PayId
			AND PayDummy.CurrencyId = CurrencyId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `PayDummy_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `PayDummy_Set`(
	IN SiteId INT, 
    IN PayId INT,
    IN CurrencyId int,
    IN Amount DECIMAL
)
BEGIN
	# 20191227@Scott (Backend, Payment Service): Set Dummy for Online Payment

	INSERT INTO PayDummy(SiteId, PayId, CurrencyId, Amount) VALUE(SiteId, PayId, CurrencyId, Amount) 
	ON DUPLICATE KEY 
	UPDATE PayDummy.Amount=PayDummy.Amount + Amount;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `PayLanguage_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `PayLanguage_Get`()
BEGIN
	SELECT 
		PL.PayId,
		PL.LangId,
		PL.PayLangId
	FROM
		PayLanguage AS PL;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `PaySetting_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `PaySetting_Get`(
	IN SiteId INT
)
BEGIN
	SELECT 
		Pay.Id AS PayId, Pay.PayName, Pay.Setting
	FROM
		PayList AS Pay
	WHERE
		Pay.`Status` = 1;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `PointLevelInfoDetail_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `PointLevelInfoDetail_Get`(
	IN SiteId INT,
    IN CurrencyId INT,
    IN PointLevel INT
)
BEGIN
	# 20200113@Tyler (BackendSite): Get PointLevelInfoDetail by SiteId and CurrencyId and PointLevel
	SELECT 
		PointLevelInfo.SiteId, 
        PointLevelInfo.CurrencyId, 
        PointLevelInfo.PointLevel, 
        PointLevelInfo.DepositLeast, 
        PointLevelInfo.BetLeast
	FROM
		PointLevelInfo
	WHERE
		PointLevelInfo.SiteId = SiteId
        AND PointLevelInfo.CurrencyId = CurrencyId
        AND PointLevelInfo.PointLevel = PointLevel;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `PointLevelInfo_Del` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `PointLevelInfo_Del`(
	IN SiteId INT,
    IN CurrencyId INT,
    IN PointLevel INT
)
BEGIN
	# 20200113@Tyler (BackendSite): Delete PointLevelInfoDetail by SiteId and CurrencyId and PointLevel
	DELETE FROM `Siebog`.PointLevelInfo 
	WHERE
		PointLevelInfo.SiteId = SiteId
		AND PointLevelInfo.CurrencyId = CurrencyId
		AND PointLevelInfo.PointLevel = PointLevel;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `PointLevelInfo_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `PointLevelInfo_Get`(
	IN SiteId INT,
    IN CurrencyId INT
)
BEGIN
	# 20200111@Tyler (BackendSite): Get PointLevelInfo by SiteId and CurrencyId
	SELECT 
		PointLevelInfo.SiteId, 
        PointLevelInfo.CurrencyId, 
        PointLevelInfo.PointLevel, 
        PointLevelInfo.DepositLeast, 
        PointLevelInfo.BetLeast
	FROM
		PointLevelInfo
	WHERE
		PointLevelInfo.SiteId = SiteId
        AND PointLevelInfo.CurrencyId = CurrencyId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `PointLevelInfo_Ins` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `PointLevelInfo_Ins`(
	IN SiteId INT,
    IN CurrencyId INT,
    IN PointLevel INT,
    IN BetLeast decimal(18,2),
    IN DepositLeast decimal(18,2)
)
BEGIN
	# 20200111@Tyler(BackendSite): Add new PointLevelInfo
	INSERT INTO PointLevelInfo(SiteId,CurrencyId,PointLevel,BetLeast,DepositLeast)
	SELECT 
		SiteId,
		CurrencyId,
		PointLevel,
		BetLeast,
		DepositLeast
	FROM DUAL WHERE
		NOT EXISTS( SELECT 
				1
			FROM
				PointLevelInfo
			WHERE
				PointLevelInfo.SiteID = SiteId
					AND PointLevelInfo.CurrencyId = CurrencyId
					AND PointLevelInfo.PointLevel = PointLevel);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `PointLevelInfo_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `PointLevelInfo_Set`(
	IN SiteId INT,
    IN CurrencyId INT,
    IN PointLevel INT,
    IN BetLeast decimal(18,2),
    IN DepositLeast decimal(18,2)
)
BEGIN
	# 20200111@Tyler(BackendSite): Edit PointLeveInfo
	UPDATE PointLevelInfo 
	SET 
        PointLevelInfo.BetLeast = BetLeast,
        PointLevelInfo.DepositLeast = DepositLeast
	WHERE
		PointLevelInfo.SiteId = SiteId
			AND PointLevelInfo.CurrencyId = CurrencyId
				AND PointLevelInfo.PointLevel = PointLevel;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `ServerInfoList_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `ServerInfoList_Get`(
)
BEGIN
	# 20200111@Scott (MemberSite): Get List of ServerInfo
	SELECT 
		ServerInfo.SiteId, 
        ServerInfo.SiteName, 
        ServerInfo.LangList, 
        ServerInfo.CurrencyList, 
        ServerInfo.DefLang,
        ServerInfo.DefCurrency
	FROM
		ServerInfo;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `ServerInfo_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `ServerInfo_Get`(
	IN SiteId INT
)
BEGIN
	# 20191129@Scott (MemberSite): Get ServerInfo
	SELECT 
		ServerInfo.SiteId, 
        ServerInfo.SiteName, 
        ServerInfo.LangList, 
        ServerInfo.CurrencyList, 
        ServerInfo.DefLang,
        ServerInfo.DefCurrency
	FROM
		ServerInfo
	WHERE
		ServerInfo.SiteId = SiteId
	LIMIT 1;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Service_EffectiveBetSum_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Service_EffectiveBetSum_Set`(
	IN CustId INT,
    IN SiteId INT,
	IN Stake DECIMAL(18,2)
)
BEGIN
	# 20200218@Scott (Service): Set EffectiveBetSum.

	INSERT INTO CustTransSummary(CustId, Site, EffectiveBetSum) VALUE(CustId, SiteId, Stake) 
	ON DUPLICATE KEY 
	UPDATE CustTransSummary.EffectiveBetSum = CustTransSummary.EffectiveBetSum + Stake;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Service_TpBetRecord_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Service_TpBetRecord_Get`(
	IN TpId INT,
    IN TransId VARCHAR(20),
    IN GameType INT
)
BEGIN
	# 20200210@Scott (Service): Get TpBetRecord
    
	SELECT 
		`TpId`,
		`TransId`,
		`GameType`,
		`TpUserName`,
		`Stake`,
		`WinlostAmount`,
		`SettlementType`,
		`TicketStatus`
	FROM
		`TpBetRecord`
	WHERE
		TpBetRecord.TpId = TpId
			AND TpBetRecord.TransId = TransId
			AND TpBetRecord.GameType = GameType FOR UPDATE;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Service_TpBetRecord_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Service_TpBetRecord_Set`(
	IN TpId INT,
    IN TransId VARCHAR(20),
    IN GameType INT,
    IN CustId INT,
    IN UserName VARCHAR(30),
    IN CurrencyId TINYINT,
    IN SiteId INT,
    IN TpUserName VARCHAR(50),
    IN TransTime BIGINT,
    IN SettledTime BIGINT,
    IN Stake DECIMAL(18,2),
    IN WinlostAmount DECIMAL(18,2),
    IN SettlementType TINYINT,
    IN TicketStatus TINYINT,
    IN OriginalState VARCHAR(10),
    IN TicketKey BIGINT
)
BEGIN
	# 20200210@Scott (MemBetSite): to Set TpBetRecord.
    
    SET @TransTime = FROM_UNIXTIME(TransTime);    
    SET @SettledTime = FROM_UNIXTIME(SettledTime);
    
	INSERT INTO TpBetRecord(TpId,TransId,GameType,CustId,UserName,CurrencyId,SiteId,TpUserName,TransTime,SettledTime,Stake,WinlostAmount,SettlementType,TicketStatus,OriginalState,TicketKey) 
		VALUES(TpId,TransId,GameType,CustId,UserName,CurrencyId,SiteId,TpUserName,@TransTime,@SettledTime,Stake,WinlostAmount,SettlementType,TicketStatus,OriginalState,TicketKey)
	ON DUPLICATE KEY 
	UPDATE TpBetRecord.TransTime=@TransTime,
		TpBetRecord.SettledTime=@SettledTime, 
		TpBetRecord.Stake=Stake, 
		TpBetRecord.WinlostAmount=WinlostAmount,
		TpBetRecord.SettlementType=SettlementType,
		TpBetRecord.TicketStatus=TicketStatus,
		TpBetRecord.OriginalState=OriginalState,
		TpBetRecord.TicketKey=TicketKey;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Service_TpTicketKey_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Service_TpTicketKey_Set`(
	IN TpId INT,
    IN TicketKey BIGINT
)
BEGIN
	# 20200210@Scott (Service): to Set TpTicketKey.

	UPDATE TpList 
	SET 
		TpList.TicketKey = TicketKey
	WHERE
		TpList.Id = TpId AND TpList.TicketKey <> TicketKey;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `SuperAgent_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `SuperAgent_Get`(
	IN CurrencyId TINYINT,
    IN SiteId INT
)
BEGIN
	SELECT 
		Customer.CustId,
		Customer.SiteId,
		Customer.UserName,
		Customer.Userpwd,
		Customer.CurrencyId,
		Customer.UserLang,
		Customer.ActStatus,
		Customer.AgentId,
		Customer.AllAgentId,
		Customer.UserLevel,
		Customer.IsTest
	FROM
		Customer
	WHERE
		Customer.CurrencyId = CurrencyId AND Customer.AgentId = 0
			AND Customer.SiteId = SiteId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `TestDate` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `TestDate`(
	IN T TIMESTAMP
)
BEGIN
	SELECT  T AS `Time`,UNIX_TIMESTAMP(NOW()) AS Time1;#FROM_UNIXTIME(T)
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `TmpPendingFund_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `TmpPendingFund_Get`(
	IN SiteId INT
)
BEGIN
	# 20191211@Eddie(BackendSite): Get Pending fund list
	SELECT Customer.SiteId,
			Customer.UserName,
			Customer.CurrencyId,
			Customer.Status,
			CustWallet.Balance,
            CustWallet.CustId
	FROM Siebog.CustWallet
	INNER JOIN Siebog.Customer 
	ON CustWallet.CustId=Customer.CustId AND
		CustWallet.SiteId = SiteId
	WHERE CustWallet.Balance > 0
    ;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `TpCurrency_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `TpCurrency_Get`(
	IN TpId INT,
    IN CurrencyId INT
)
BEGIN
	# 20191117@Scott (ThirdParty Service): Get Tp Currency
	SELECT 
		TpCurrency.TpCurrencyId, TpCurrency.ExchangeRate
	FROM
		TpCurrency
	WHERE
		TpCurrency.TpId = TpId
			AND TpCurrency.CurrencyId = CurrencyId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `TpCustInfo_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `TpCustInfo_Get`(
	IN TpId INT,
	IN CustId INT
)
BEGIN

	# 20191211@Scott (Third Party Service): Get ThirdParty Cust Info
    # 20191228@Scott (Third Party Service): Added ExchangeRate
	SELECT 
		TC.TpId,
		TC.CustId,
		TC.SiteId,
		TC.UserName,
		TC.TpUserName,
		TC.TpPwd,
        TCY.TpCurrencyId,
        TCY.ExchangeRate,
		C.IsTest,
        C.ActStatus,
		TC.`Status`,
        C.`Status` AS CStatus
	FROM
		TpCurrency AS TCY
			INNER JOIN
		Customer AS C ON C.CurrencyId = TCY.CurrencyId
			RIGHT OUTER JOIN
		TpCustInfo AS TC ON TC.CustId = C.CustId AND TC.TpId = TCY.TpId
	WHERE
		TC.TpId = TpId AND TC.CustId = CustId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `TpCustInfo_Ins` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `TpCustInfo_Ins`(
	IN TpId INT,
    IN CustId INT,
    IN UserName VARCHAR(30),
    IN SiteId INT,
    IN TpUserName VARCHAR(50),
    IN TpPwd VARCHAR(50)
)
BEGIN
	INSERT INTO `Siebog`.`TpCustInfo`
	(`TpId`,
	`CustId`,
	`UserName`,
	`SiteId`,
	`TpUserName`,
	`TpPwd`)
	SELECT 
		TpId,
		CustId,
		UserName,
		SiteId,
		TpUserName,
		TpPwd
	FROM DUAL WHERE
		NOT EXISTS( SELECT 
				1
			FROM
				TpCustInfo
			WHERE
				TpCustInfo.TpId = TpId
					AND TpCustInfo.CustId = CustId);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `TpLangId_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `TpLangId_Get`(
	IN TpId INT,
    IN LangId VARCHAR(4)
)
BEGIN
	SELECT 
		`TpLanguage`.`TpLangId`
	FROM
		`Siebog`.`TpLanguage`
	WHERE
		`TpLanguage`.`TpId` = TpId
			AND `TpLanguage`.`LangId` = LangId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `TpLastLoginTime_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `TpLastLoginTime_Set`(	
	IN TpId INT,
    IN CustId INT
)
BEGIN
	# 20191228@Scott (ThirdParty Service): Set LastLoginTime for TpCustInfo

	UPDATE TpCustInfo 
	SET 
		LastLoginTime = NOW()
	WHERE
		TpCustInfo.TpId = TpId
			AND TpCustInfo.CustId = CustId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `TpLastTradingTime_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `TpLastTradingTime_Set`(	
	IN TpId INT,
    IN CustId INT
)
BEGIN
	# 20191228@Scott (ThirdParty Service): Set LastTradingTime for TpCustInfo

	UPDATE TpCustInfo 
	SET 
		LastTradingTime = NOW()
	WHERE
		TpCustInfo.TpId = TpId
			AND TpCustInfo.CustId = CustId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `TpTransferInfo_Ins` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `TpTransferInfo_Ins`(
	IN TpId INT,
	IN CustId INT,
	IN UserName VARCHAR(30),
	IN SiteId INT,
	IN Direction TINYINT,
	IN Amount DECIMAL(18,2),
	IN Memo VARCHAR(100),
	IN ThirdId VARCHAR(50),
    IN Prefix CHAR(2)
)
BEGIN
	# 20191117@Scott (MemberSite): Funds Transfer Request
	# 20191212@Scott (MemberSite): Removed Fill 0 for TransId.
	SET @TransferId=0;
	INSERT INTO `Siebog`.`TpTransferInfo`
	(`TpId`,
	`CustId`,
	`UserName`,
	`SiteId`,
	`Direction`,
	`Amount`,
	`Memo`,
	`Status`,
	`ThirdId`)
	VALUES
	(TpId,
	CustId,
	UserName,
	SiteId,
	Direction,
	Amount,
	Memo,
	2,
	ThirdId);
    
	SET @TransferId=LAST_INSERT_ID();
	SET @TransId=CONCAT(Prefix, @TransferId);
	UPDATE TpTransferInfo 
	SET 
		TpTransferInfo.TransId = @TransId
	WHERE
		TpTransferInfo.TransferId = @TransferId;
	SELECT @TransId AS TransId;    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `TpTransferInfo_Pending_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `TpTransferInfo_Pending_Get`()
BEGIN
	# 20191117@Scott (ThirdParty Service): Get TpTransferInf for Pending Status
	SELECT 
		T.TransId,
		T.TpId,
		T.ThirdId,
		T.CustId,
		T.Direction,
		T.Amount,
		T.`Status`,
		DATE_FORMAT(T.CreateTime, '%Y-%m-%dT%TZ') AS CreateTime
	FROM
		TpTransferInfo AS T
	WHERE
		T.`Status` IN (2 , 3)
			AND DATE_ADD(T.CreateTime,
			INTERVAL 3 MINUTE) < NOW();
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `TpTransferInfo_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `TpTransferInfo_Set`(
	IN TransferId INT,
    IN ThirdId VARCHAR(50),
	IN Memo VARCHAR(100),
    IN ForStatus TINYINT,
    IN `Status` TINYINT
)
BEGIN
	UPDATE `Siebog`.`TpTransferInfo` 
	SET 
		`TpTransferInfo`.`Memo` = Memo,
		`TpTransferInfo`.`Status` = `Status`,
        `TpTransferInfo`.`ThirdId` = IFNULL(ThirdId, `TpTransferInfo`.`ThirdId`)
	WHERE
		`TpTransferInfo`.`TransferId` = TransferId
			AND `TpTransferInfo`.`Status` = ForStatus;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `TpWallet_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `TpWallet_Get`(
	IN CustId INT
)
BEGIN
	# 20200114@Scott (Third Party Service): Get ThirdParty Wallet List

	SELECT 
		TC.TpId, 
        TL.TpName, 
        TC.TpUserName
	FROM
		Customer AS C
			INNER JOIN
		TpCustInfo AS TC ON TC.CustId = C.CustId
			INNER JOIN
		TpList AS TL ON TL.Id = TC.TpId
	WHERE
		TC.CustId = CustId; #AND C.ActStatus = 2
			#AND TC.LastTradingTime IS NOT NULL;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Tp_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Tp_Get`(
	IN TpId INT
)
BEGIN
	SELECT 
		`TpList`.`Id`,
		`TpList`.`TpName`,
		`TpList`.`Setting`,
		`TpList`.`Status`
	FROM
		`Siebog`.`TpList`
	WHERE
		`TpList`.`Id` = TpId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `VipLevel_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `VipLevel_Get`(
	IN CustId INT
)
BEGIN
	SET @UserLevel = 0;
    SET @CurrencyId = 0;
    /*Get User Level*/
	SELECT 
		Customer.UserLevel,
        Customer.CurrencyId
	INTO @UserLevel, @CurrencyId FROM
		Customer
	WHERE
		Customer.CustId = CustId;
		
	SET @DepositSum = 0;
	SET @EffectiveBetSum = 0;
    
	/*Get Turnover*/
	SELECT 
		CustTransSummary.DepositSum,
		CustTransSummary.EffectiveBetSum
	INTO @DepositSum , @EffectiveBetSum FROM
		CustTransSummary
	WHERE
		CustTransSummary.CustId = CustId;
    
    /*Get Point Level*/
	SELECT 
		CustTransSummary.DepositSum,
		CustTransSummary.EffectiveBetSum
	INTO @DepositSum , @EffectiveBetSum FROM
		CustTransSummary
	WHERE
		CustTransSummary.CustId = CustId;        

	SET @PointLevel=0;
	SELECT 
		MAX(PointLevelInfo.PointLevel)
	INTO @PointLevel FROM
		PointLevelInfo
	WHERE
		@DepositSum >= PointLevelInfo.DepositLeast
			AND @EffectiveBetSum > PointLevelInfo.BetLeast
			AND PointLevelInfo.CurrencyId = @CurrencyId;
	/*Get larger Level be Vip Level*/
	SELECT 
    IF(@UserLevel > IFNULL(@PointLevel, 0),
        @UserLevel,
        @PointLevel) AS VipLevel;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `Wallet_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `Wallet_Set`(
	IN CustId INT,
    IN UserName VARCHAR(30),
    IN SiteId INT,
	IN Amount DECIMAL(18,2),
    IN `TYPE` SMALLINT,
    IN Memo VARCHAR(200),
    IN RefTransId VARCHAR(45),
    OUT Is1stDepoist BOOLEAN,
    OUT AfterAmount DECIMAL(18,4)
)
BEGIN
	# 20191129@Scott (MemberSite): Set Wallet
    # 20191211@Scott (MemberSite): Fixed Bug for AfterAmount Should be >=0
    # 20191211@Scott (MemberSite, Backend): Added Type for 1002,1003
    
	SET AfterAmount = Amount;
    SET Is1stDepoist = FALSE;
    SELECT TRUE INTO Is1stDepoist FROM Customer WHERE Customer.CustId=CustId AND Customer.ActStatus=1 FOR UPDATE;
    SELECT Amount + CustWallet.Balance INTO AfterAmount FROM CustWallet WHERE CustWallet.CustId=CustId FOR UPDATE;
	
	IF AfterAmount >= 0 AND CustId > 0 THEN
		SET @Direction = 2;
        
		IF Amount > 0 THEN
			SET @Direction = 1;
        END IF;
        
		INSERT INTO FinancialLog (CustId,UserName,SiteId,`Type`,Direction,ChangeAmount,BeforeAmount,AfterAmount,Memo,RefTransId) VALUES
		(CustId, UserName, SiteId,`Type`, @Direction, ABS(Amount), AfterAmount - Amount, AfterAmount, Memo, RefTransId);
        
		INSERT INTO CustWallet(CustId, SiteId, Balance) VALUE(CustId, SiteId, AfterAmount) 
		ON DUPLICATE KEY 
		UPDATE CustWallet.Balance=AfterAmount;
        
        IF `TYPE` BETWEEN 1 AND 2000 AND `TYPE` NOT IN (2,3) /*Adjustment*/ THEN
			INSERT INTO CustTransSummary
			(CustId,
			Site,
			DepositSum,
			WithdrawlSum)
			VALUES
			(CustId,
			SiteId,
			IF(`TYPE` BETWEEN 1 AND 1000, Amount, 0),
			IF(`TYPE` BETWEEN 1001 AND 2000, IF(`TYPE` BETWEEN 1002 AND 1003/*1002, 1003 is void Withdrawl*/, ABS(Amount) * -1, ABS(Amount)), 0))

			ON DUPLICATE KEY 
			UPDATE CustTransSummary.DepositSum = IF(`TYPE` BETWEEN 1 AND 1000, CustTransSummary.DepositSum + Amount, CustTransSummary.DepositSum),
			CustTransSummary.WithdrawlSum = IF(`TYPE` BETWEEN 1001 AND 2000, CustTransSummary.WithdrawlSum + IF(`TYPE` BETWEEN 1002 AND 1003/*1002, 1003 is void Withdrawl*/, ABS(Amount) * -1, ABS(Amount)), CustTransSummary.WithdrawlSum);
		END IF;            

		IF Is1stDepoist THEN
			UPDATE Customer SET Customer.ActStatus = 2 WHERE Customer.CustId = CustId AND Customer.ActStatus = 1;
        END IF;
    END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `WithdrawalInfoRecent_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `WithdrawalInfoRecent_Get`(
	IN CustId INT,
    IN SiteId INT,
    IN TopNum INT
)
BEGIN
	# 20191215@Scott(MemberSite): Get List of WithdrawalInfo for Recent Transcation
    
	SELECT 
		DATE_FORMAT(WithdrawalInfo.CreateTime, '%Y-%m-%dT%TZ') AS CreateTime,
		WithdrawalInfo.TransId,
		WithdrawalInfo.Amount,
        WithdrawalInfo.ChargeFee,
		WithdrawalInfo.`Status`,
        WithdrawalInfo.WithdrawalInfo
	FROM
		WithdrawalInfo
	WHERE
		WithdrawalInfo.CustId = CustId
			AND WithdrawalInfo.SiteId = SiteId
	ORDER BY WithdrawalId DESC
	LIMIT TopNum;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `WithdrawalInfoSearch_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `WithdrawalInfoSearch_Get`(
    IN CustId INT,    
    IN StartDate BIGINT,
    IN EndDate BIGINT,
    IN SiteId INT,
	IN PageSize INT,
    INOUT PageNumber INT,
    OUT TotalPages INT,
    OUT RowCount INT,
    OUT StartIndex INT
)
BEGIN
	# 20191219@Scott (MemberSite): Get WithdrawalInfo for History
	SET PageSize = ABS(PageSize);
    SET PageNumber = ABS(PageNumber);
    SET RowCount=0;
    #FROM_UNIXTIME
    SET @StartDate = FROM_UNIXTIME(StartDate);
    SET @EndDate = FROM_UNIXTIME(EndDate);
    IF StartDate IS NOT NULL AND EndDate IS NOT NULL THEN
		SELECT 
			COUNT(1)
		INTO RowCount FROM
			WithdrawalInfo
		WHERE
			(WithdrawalInfo.CreateTime BETWEEN @StartDate AND @EndDate)
				AND WithdrawalInfo.SiteId = SiteId AND WithdrawalInfo.CustId=CustId;
    
    	IF RowCount = 0 THEN 
			SET PageNumber = 1;
			SET TotalPages = 0;
		ELSE
			SET TotalPages = CEIL(RowCount/PageSize);		
			IF PageNumber > TotalPages THEN 
				SET PageNumber = TotalPages;
			END IF;
			
			SET StartIndex = (PageNumber - 1) * PageSize; 
            
			SELECT 
				WithdrawalInfo.TransId, 
                WithdrawalInfo.Amount, 
                WithdrawalInfo.ChargeFee, 
                WithdrawalInfo.WithdrawalInfo,                 
                DATE_FORMAT(WithdrawalInfo.CreateTime, '%Y-%m-%dT%TZ') AS CreateTime,
                WithdrawalInfo.`Status`
			FROM
				WithdrawalInfo
			WHERE
				(WithdrawalInfo.CreateTime BETWEEN @StartDate AND @EndDate)
					AND WithdrawalInfo.SiteId = SiteId AND WithdrawalInfo.CustId=CustId
			LIMIT StartIndex , PageSize;
		END IF;
	END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `WithdrawalInfo_Ins` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `WithdrawalInfo_Ins`(
    IN CustId INT,
    IN UserName VARCHAR(30),
	IN CurrencyId TINYINT,    
    IN SiteId INT ,
    IN WithdrawalInfo JSON,
    IN Amount DECIMAL(18,4),
    IN Memo VARCHAR(100),
    IN Prefix CHAR(2),
    OUT vTransId VARCHAR(30)
    )
BEGIN
	# 20191129@Scott (MemberSite): Withdrawal Request
    # 20191212@Scott (MemberSite): Removed Fill 0 for TransId.
    
	SET @WithdrawalId = 0;

	INSERT INTO WithdrawalInfo(CustId,UserName,CurrencyId,SiteId,WithdrawalInfo,Amount,Memo,`Status`)
	VALUES(CustId,UserName,CurrencyId,SiteId,WithdrawalInfo,Amount,Memo,1);
    
	SET @WithdrawalId=LAST_INSERT_ID();
	SET @TransId=CONCAT(Prefix, @WithdrawalId);
    
	UPDATE WithdrawalInfo 
	SET 
		WithdrawalInfo.TransId = @TransId
	WHERE
		WithdrawalInfo.WithdrawalId = @WithdrawalId;
        
	SET vTransId = @TransId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `WithdrawalInfo_Set` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `WithdrawalInfo_Set`(
	IN WithdrawalId INT,
    IN SiteId INT,
    IN `Comment` VARCHAR(500),
    IN ForStatus JSON,
    IN `Status` TINYINT,
    IN WithdrawalInfo JSON,
    IN WithdrawalType TINYINT,
    IN RefId VARCHAR(45),
    OUT CustId INT,
    OUT UserName VARCHAR(30),
    OUT Amount decimal
)
BEGIN
	# 20191203@Eddie (BackendSite): WithdrawalInfo to do Approved
	SET @WithdrawalId = 0;
    
    SELECT 
		WithdrawalInfo.CustId, WithdrawalInfo.UserName, WithdrawalInfo.Amount ,WithdrawalInfo.WithdrawalId
	INTO CustId , UserName, Amount, @WithdrawalId FROM
		WithdrawalInfo
	WHERE
		WithdrawalInfo.WithdrawalId = WithdrawalId
            AND JSON_CONTAINS(ForStatus , CONVERT(WithdrawalInfo.`Status`,CHAR))=1
			AND WithdrawalInfo.SiteId = SiteId
	FOR UPDATE NOWAIT;
    
    IF @WithdrawalId<>0 THEN
		UPDATE WithdrawalInfo 
		SET 
			WithdrawalInfo.`Status` = IF(`Status` = 0,
				WithdrawalInfo.`Status`,
				`Status`),
			WithdrawalInfo.`Comment` = `Comment`,
			WithdrawalInfo.WithdrawalInfo = IFNULL(WithdrawalInfo,
					WithdrawalInfo.WithdrawalInfo),
			WithdrawalInfo.WithdrawalType = IFNULL(WithdrawalType , WithdrawalInfo.WithdrawalType),
            WithdrawalInfo.RefId = IFNULL(RefId, WithdrawalInfo.RefId)
		WHERE
			WithdrawalInfo.WithdrawalId = @WithdrawalId;
	END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `WithdrawalList_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `WithdrawalList_Get`(
	IN SiteId INT,
    IN WithdrawalType INT,
    IN StartDate BIGINT,
    IN EndDate BIGINT,    
    IN WithdrawalId INT,
	IN TransId VARCHAR(30),
    IN UserName VARCHAR(30),
    IN CurrencyId TINYINT,
    IN `Status` INT,
	IN PageSize INT,
    INOUT PageNumber INT,
    OUT TotalPages INT,
    OUT RowCount INT,
    OUT StartIndex INT    
)
BEGIN
   	# 20191125@Eddie Wu (BackendSite): Get List of WithdrawInfo
    # 20200104@Scott (BackendSite): Add conditions
    
	SET @StartDate = FROM_UNIXTIME(StartDate);
    SET @EndDate = FROM_UNIXTIME(EndDate);
    
	SELECT 
		COUNT(1)
	INTO RowCount FROM
		WithdrawalInfo AS WI
	WHERE
		WI.SiteId = SiteId
			AND WI.WithdrawalType = IFNULL(WithdrawalType, WI.WithdrawalType)
			AND WI.CreateTime BETWEEN @StartDate AND @EndDate
			AND (WI.TransId = IFNULL(TransId, WI.TransId)
			OR WI.WithdrawalId = IFNULL(WithdrawalId, WI.WithdrawalId))
			AND WI.UserName = IFNULL(UserName, WI.UserName)
            AND WI.CurrencyId = IFNULL(CurrencyId, WI.CurrencyId)
			AND WI.`Status` = IFNULL(`Status`, WI.`Status`);
            
    	IF RowCount = 0 THEN 
			SET PageNumber = 1;
			SET TotalPages = 0;
		ELSE
			SET TotalPages = CEIL(RowCount/PageSize);		
			IF PageNumber > TotalPages THEN 
				SET PageNumber = TotalPages;
			END IF;
			
			SET StartIndex = (PageNumber - 1) * PageSize; 

			SELECT 
				WI.WithdrawalId,
				WI.TransId,
                SI.SiteName,
				WI.WithdrawalType,
				WI.CustId,
				WI.UserName,
                WI.CurrencyId,
				WI.SiteId,
				WI.WithdrawalInfo,
				WI.Amount,
				#WI.ChargeFee,
				DATE_FORMAT(CreateTime, '%Y-%m-%dT%TZ') AS CreateTime,
				WI.Memo,
				#WI.`Comment`,
				WI.`Status`,
				WI.WithdrawalInfo->>'$.AccountNo' AS AccountNo,
				WI.WithdrawalInfo->>'$.AccountName' AS AccountName,
				BIS2.BankName AS FromBankName,
				BIS2.BankCode AS FromBankCode,
				WI.WithdrawalInfo->>'$.FromAccountNo' AS FromAccountNo,
				WI.WithdrawalInfo->>'$.FromAccountName' AS FromAccountName                        
			FROM
				WithdrawalInfo AS WI
				INNER JOIN 
				ServerInfo AS SI ON SI.SiteId = WI.SiteId                
				LEFT OUTER JOIN 
				BankInfoSys AS BIS2 ON WI.WithdrawalInfo->'$.FromSysId'= BIS2.Id                
			WHERE
				WI.SiteId = SiteId
					AND WI.WithdrawalType = IFNULL(WithdrawalType, WI.WithdrawalType)
					AND WI.CreateTime BETWEEN @StartDate AND @EndDate
					AND (WI.TransId = IFNULL(TransId, WI.TransId)
					OR WI.WithdrawalId = IFNULL(WithdrawalId, WI.WithdrawalId))
					AND WI.UserName = IFNULL(UserName, WI.UserName)
					AND WI.CurrencyId = IFNULL(CurrencyId, WI.CurrencyId)
					AND WI.`Status` = IFNULL(`Status`, WI.`Status`)
			ORDER BY WI.CreateTime
			LIMIT StartIndex , PageSize;
		END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `WithdrawalLog_Ins` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `WithdrawalLog_Ins`(
	IN WithdrawalId INT,
    IN SiteId INT,
    IN `Action` INT,
    IN `Comment` VARCHAR(500),
    IN Operator VARCHAR(30)
)
BEGIN
	# 20191203@Eddie (BackendSite): to Record WithdrawalLog.
	INSERT INTO `WithdrawalLog`
		(WithdrawalId,
		`SiteId`,
		`Action`,
		`Comment`,
        Operator
        )
	VALUES 
		(WithdrawalId,
		SiteId,
		`Action`,
		`Comment`,
        Operator
		);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `WithdrawalRequesPay_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `WithdrawalRequesPay_Get`(
	IN WithdrawalId INT
)
BEGIN
    # 20200106@Scott (Payment Service): WithdrawalReques for Online Pay
    
	SELECT 
		WI.TransId,
        WI.SiteId,
        PL.Id  AS PayId,
        WI.CurrencyId,
		PL.PayName,
		PL.Setting
	FROM
		WithdrawalInfo AS WI 
		INNER JOIN 
        PayList AS PL ON WI.WithdrawalInfo->'$.PayId'= PL.Id        
	WHERE
		WI.WithdrawalId = WithdrawalId
        AND WI.WithdrawalType=1 AND WI.`Status` NOT IN (6 , 7);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `WithdrawalRequestComment_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `WithdrawalRequestComment_Get`(
	IN WithdrawalId INT,
    IN SiteId INT
)
BEGIN
	# 20200104@Scott (BackendSite): Added Get Operator
	SELECT 
		DATE_FORMAT(CreateTime, '%Y-%m-%dT%TZ') AS CreateTime,
        `Comment`,
        `Action`,
        Operator
	FROM
		WithdrawalLog
	WHERE
		WithdrawalLog.WithdrawalId = WithdrawalId
			AND WithdrawalLog.SiteId = SiteId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS `WithdrawalRequestTransID_Get` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`%` PROCEDURE `WithdrawalRequestTransID_Get`(
	IN WithdrawalId INT,
    IN SiteId INT
)
BEGIN
   	# 20191125@Eddie Wu (BackendSite): Get WithdrawalInfo by WithdrawalId
    # 20200104@Scott (BackendSite): Adjust to Join ServerInfo.
    
	SELECT WI.WithdrawalId,
		WI.TransId,
		WI.WithdrawalType,
		WI.CustId,
		WI.UserName,
        CP.Email,
        WI.CurrencyId,
		WI.SiteId,
		WI.WithdrawalInfo,
		WI.Amount,
		#WI.ChargeFee,
        WI.`Comment`,
		DATE_FORMAT(WI.CreateTime, '%Y-%m-%dT%TZ') AS CreateTime,
		WI.Memo,
		WI.`Status`,
        WI.RefId,
        SI.SiteName,
        BIS.BankName,
        BIS.BankCode,
		WI.WithdrawalInfo->'$.SysId' AS SysId,
        WI.WithdrawalInfo->'$.PayId' AS PayId,
        WI.WithdrawalInfo->'$.BankId' AS BankId,
        WI.WithdrawalInfo->>'$.AccountNo' AS AccountNo,
        WI.WithdrawalInfo->>'$.AccountName' AS AccountName,
        PL.PayName,
        BIS2.BankName AS FromBankName,
        BIS2.BankCode AS FromBankCode,
        WI.WithdrawalInfo->>'$.FromAccountNo' AS FromAccountNo,
        WI.WithdrawalInfo->>'$.FromAccountName' AS FromAccountName        
	FROM
		WithdrawalInfo AS WI 
        LEFT OUTER JOIN 
        CustProfile AS CP ON CP.CustId = WI.CustId
        INNER JOIN 
		ServerInfo AS SI ON SI.SiteId = WI.SiteId
		INNER JOIN 
        BankInfoSys AS BIS ON WI.WithdrawalInfo->'$.SysId'= BIS.Id
		LEFT OUTER JOIN 
        BankInfoSys AS BIS2 ON WI.WithdrawalInfo->'$.FromSysId'= BIS2.Id
		LEFT OUTER JOIN 
        PayList AS PL ON WI.WithdrawalInfo->'$.PayId'= PL.Id        
	WHERE
		WI.WithdrawalId = WithdrawalId
			AND WI.SiteId = SiteId;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-02-19 16:06:38
