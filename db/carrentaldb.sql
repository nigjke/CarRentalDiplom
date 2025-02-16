-- MySQL dump 10.13  Distrib 8.0.40, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: carrentaldb
-- ------------------------------------------------------
-- Server version	8.0.40

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
-- Table structure for table `cars`
--

DROP TABLE IF EXISTS `cars`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cars` (
  `car_id` int NOT NULL AUTO_INCREMENT,
  `make` varchar(50) DEFAULT NULL,
  `model` varchar(50) DEFAULT NULL,
  `year` year DEFAULT NULL,
  `license_plate` varchar(20) DEFAULT NULL,
  `status` enum('Свободная','Занята') DEFAULT NULL,
  `price` int DEFAULT NULL,
  `photo` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`car_id`),
  UNIQUE KEY `license_plate` (`license_plate`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cars`
--

LOCK TABLES `cars` WRITE;
/*!40000 ALTER TABLE `cars` DISABLE KEYS */;
INSERT INTO `cars` VALUES (1,'Toyota','Camry',2022,'А123ВС77','Свободная',3000,'toyota_camry.jpg'),(2,'BMW','X5',2021,'В456АС78','Занята',5000,'bmw_x5.jpg'),(3,'Mercedes','E200',2020,'С789ВА79','Свободная',4500,'mercedes_e200.jpg'),(4,'Audi','Q7',2019,'Д111АС80','Занята',6000,'audi_q7.jpg'),(5,'Honda','Civic',2018,'Е222ВС81','Свободная',2500,'honda_civic.jpg'),(6,'Ford','Focus',2017,'Ж333АС82','Свободная',2000,'ford_focus.jpg'),(7,'Nissan','X-Trail',2022,'З44483','Занята',5500,'nissan_xtrail.jpg'),(8,'Hyundai','Tucson',2022,'И555АС84','Свободная',4000,'hyundai_tucson.jpg'),(9,'Mazda','CX-5',2021,'К666ВС85','Занята',4200,'mazda_cx5.jpg'),(10,'Kia','Sportage',2020,'Л777АС86','Свободная',3800,'kia_sportage.jpg');
/*!40000 ALTER TABLE `cars` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `customers`
--

DROP TABLE IF EXISTS `customers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `customers` (
  `customer_id` int NOT NULL AUTO_INCREMENT,
  `first_name` varchar(50) DEFAULT NULL,
  `last_name` varchar(50) DEFAULT NULL,
  `phone` varchar(20) DEFAULT NULL,
  `driver_license` varchar(20) DEFAULT NULL,
  `passport` varchar(20) DEFAULT NULL,
  `email` varchar(50) DEFAULT NULL,
  `registration_date` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`customer_id`),
  UNIQUE KEY `driver_license` (`driver_license`),
  UNIQUE KEY `passport` (`passport`),
  UNIQUE KEY `email` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `customers`
--

LOCK TABLES `customers` WRITE;
/*!40000 ALTER TABLE `customers` DISABLE KEYS */;
INSERT INTO `customers` VALUES (1,'Иван','Иванов','+7(900)111-2233','77AA123456','5000 123456',NULL,'2025-02-14 23:05:54'),(2,'Петр','Петров','+7(901)222-3344','78BB234567','5001 234567',NULL,'2025-02-14 23:05:54'),(3,'Сергей','Сергеев','+7(902)333-4455','79CC345678','5002 345678',NULL,'2025-02-14 23:05:54'),(4,'Анна','Смирнова','+7(903)444-5566','80DD456789','5003 456789',NULL,'2025-02-14 23:05:54'),(5,'Ольга','Козлова','+7(904)555-6677','81EE567890','5004 567890',NULL,'2025-02-14 23:05:54'),(6,'Дмитрий','Федоров','+7(905)666-7788','82FF678901','5005 678901',NULL,'2025-02-14 23:05:54'),(7,'Екатерина','Васильева','+7(906)777-8899','83GG789012','5006 789012',NULL,'2025-02-14 23:05:54'),(8,'Александр','Морозов','+7(907)888-9900','84HH890123','5007 890123',NULL,'2025-02-14 23:05:54'),(9,'Марина','Николаева','+7(908)999-0011','85II901234','5008 901234',NULL,'2025-02-14 23:05:54'),(10,'Артем','Зайцев','+7(909)000-1122','86JJ012345','5009 012345',NULL,'2025-02-14 23:05:54'),(11,'213213','2312312','+7 (231) 232-1312','21 31 231231','22 31 232321',NULL,'2025-02-15 12:37:31');
/*!40000 ALTER TABLE `customers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `employee`
--

DROP TABLE IF EXISTS `employee`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `employee` (
  `employee_id` int NOT NULL AUTO_INCREMENT,
  `role_id` int DEFAULT NULL,
  `first_name` varchar(25) DEFAULT NULL,
  `last_name` varchar(25) DEFAULT NULL,
  `phone` varchar(50) DEFAULT NULL,
  `email` varchar(50) DEFAULT NULL,
  `employeeLogin` varchar(45) DEFAULT NULL,
  `employeePass` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`employee_id`),
  UNIQUE KEY `email` (`email`),
  UNIQUE KEY `employeeLogin` (`employeeLogin`),
  KEY `role_id` (`role_id`),
  CONSTRAINT `employee_ibfk_1` FOREIGN KEY (`role_id`) REFERENCES `role` (`role_id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `employee`
--

LOCK TABLES `employee` WRITE;
/*!40000 ALTER TABLE `employee` DISABLE KEYS */;
INSERT INTO `employee` VALUES (1,1,'Админ','Админов','+7 (945) 274-3627','admin@example.com','admin','admin'),(2,2,'Мария','Петрова','+7(901)2345678','maria@example.com','maria','maria'),(3,1,'Дмитрий','Сидоров','+7(902)3456789','dmitriy@example.com','dmitriy','dmitriy'),(4,2,'Екатерина','Смирнова','+7(903)4567890','ekaterina@example.com','ekaterina','ekaterina'),(5,1,'Василий','Камоцкий','+7(904)5678901','vasiliy@example.com','vasiliy','vasiliy'),(6,2,'Ольга','Кузнецова','+7(905)6789012','olga@example.com','olga','olga'),(7,1,'Андрей','Попов','+7(906)7890123','andrey@example.com','andrey','andrey'),(8,2,'Елена','Лебедева','+7(907)8901234','elena@example.com','elena','elena'),(9,2,'Менеджер','Менеджеров','+7 (238) 942-3483','manager@example.com','manager','manager'),(10,1,'Геннадий','Генов','+7 (239) 493-2482','gena@example.com','gena','gena');
/*!40000 ALTER TABLE `employee` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fines`
--

DROP TABLE IF EXISTS `fines`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fines` (
  `fine_id` int NOT NULL AUTO_INCREMENT,
  `rental_id` int DEFAULT NULL,
  `description` text,
  `fine_amount` decimal(10,2) DEFAULT NULL,
  `fine_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `is_paid` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`fine_id`),
  KEY `rental_id` (`rental_id`),
  CONSTRAINT `fines_ibfk_1` FOREIGN KEY (`rental_id`) REFERENCES `rentals` (`rental_id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fines`
--

LOCK TABLES `fines` WRITE;
/*!40000 ALTER TABLE `fines` DISABLE KEYS */;
INSERT INTO `fines` VALUES (1,1,'Нарушение правил парковки',150.00,'2025-01-05 14:30:00',1),(2,2,'Превышение скорости',200.00,'2025-01-10 15:00:00',0),(3,3,'Нарушение правил проезда',100.00,'2025-01-15 16:30:00',1),(4,4,'Парковка в запрещенной зоне',250.00,'2025-02-01 09:00:00',0),(5,5,'Неиспользование ремня безопасности',75.00,'2025-01-20 10:00:00',1),(6,6,'Превышение скорости',180.00,'2025-01-25 11:30:00',0),(7,7,'Нарушение правил парковки',120.00,'2025-02-05 12:00:00',1),(8,8,'Парковка в запрещенной зоне',300.00,'2025-02-08 13:30:00',0),(9,9,'Нарушение правил проезда',200.00,'2025-02-12 14:30:00',1),(10,10,'Неиспользование ремня безопасности',50.00,'2025-02-14 15:00:00',0);
/*!40000 ALTER TABLE `fines` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `insurance`
--

DROP TABLE IF EXISTS `insurance`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `insurance` (
  `insurance_id` int NOT NULL AUTO_INCREMENT,
  `car_id` int DEFAULT NULL,
  `insurance_provider` varchar(50) DEFAULT NULL,
  `policy_number` varchar(50) DEFAULT NULL,
  `expiration_date` date DEFAULT NULL,
  PRIMARY KEY (`insurance_id`),
  UNIQUE KEY `policy_number` (`policy_number`),
  KEY `car_id` (`car_id`),
  CONSTRAINT `insurance_ibfk_1` FOREIGN KEY (`car_id`) REFERENCES `cars` (`car_id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `insurance`
--

LOCK TABLES `insurance` WRITE;
/*!40000 ALTER TABLE `insurance` DISABLE KEYS */;
INSERT INTO `insurance` VALUES (1,1,'СберСтрахование','ABC123456','2025-12-31'),(2,2,'РЕСО-Гарантия','XYZ987654','2025-11-30'),(3,3,'Ингосстрах','LMN543210','2025-10-31'),(4,4,'ВСК','DEF456789','2025-09-30'),(5,5,'Альфастрахование','JKL112233','2025-08-31'),(6,6,'Тинькофф Страхование','MNO667788','2025-07-31'),(7,1,'СберСтрахование','QRS334455','2026-01-15'),(8,2,'Ингосстрах','PQR889900','2026-02-28'),(9,3,'РЕСО-Гарантия','STU123456','2026-03-31'),(10,4,'ВСК','VWX112233','2026-04-30');
/*!40000 ALTER TABLE `insurance` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `maintenance`
--

DROP TABLE IF EXISTS `maintenance`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `maintenance` (
  `maintenance_id` int NOT NULL AUTO_INCREMENT,
  `car_id` int DEFAULT NULL,
  `description` text,
  `maintenance_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `cost` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`maintenance_id`),
  KEY `car_id` (`car_id`),
  CONSTRAINT `maintenance_ibfk_1` FOREIGN KEY (`car_id`) REFERENCES `cars` (`car_id`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `maintenance`
--

LOCK TABLES `maintenance` WRITE;
/*!40000 ALTER TABLE `maintenance` DISABLE KEYS */;
INSERT INTO `maintenance` VALUES (1,1,'Замена масла и фильтров','2024-01-05 00:00:00',5000.00),(2,2,'ТО 20 000 км','2024-02-10 00:00:00',10000.00),(3,3,'Ремонт подвески','2024-03-18 00:00:00',25000.00),(4,4,'Замена тормозных колодок','2024-04-22 00:00:00',8000.00),(5,5,'Диагностика двигателя','2024-05-15 00:00:00',7000.00),(6,6,'Заправка кондиционера','2024-06-07 00:00:00',3000.00),(7,7,'Полная мойка и химчистка салона','2024-07-20 00:00:00',5000.00),(8,8,'Замена аккумулятора','2024-08-25 00:00:00',12000.00),(9,9,'Шиномонтаж и балансировка','2024-09-14 00:00:00',6000.00),(10,10,'Плановое ТО','2024-10-10 00:00:00',9000.00),(11,5,'Проверка системы охлаждения','2025-02-13 08:30:00',140.00),(12,6,'Замена свечей зажигания','2025-02-14 10:00:00',80.00),(13,1,'Регулировка угла наклона фар','2025-02-14 13:30:00',60.00),(14,2,'Замена тормозных колодок','2025-02-15 09:00:00',130.00),(15,3,'Очистка и смазка тормозных механизмов','2025-02-16 11:00:00',100.00),(16,4,'Проверка состояния шин и давления','2025-02-18 14:00:00',40.00),(17,5,'Ремонт выхлопной системы','2025-02-19 10:30:00',250.00),(18,6,'Замена топливного фильтра','2025-02-20 12:00:00',70.00),(19,1,'Обслуживание трансмиссии','2025-02-21 13:30:00',220.00),(20,2,'Ремонт рулевого управления','2025-02-22 09:00:00',180.00);
/*!40000 ALTER TABLE `maintenance` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `payments`
--

DROP TABLE IF EXISTS `payments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `payments` (
  `payment_id` int NOT NULL AUTO_INCREMENT,
  `rental_id` int DEFAULT NULL,
  `amount` decimal(10,2) DEFAULT NULL,
  `payment_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `payment_method` enum('Наличные','Карта','Перевод') DEFAULT NULL,
  PRIMARY KEY (`payment_id`),
  KEY `rental_id` (`rental_id`),
  CONSTRAINT `payments_ibfk_1` FOREIGN KEY (`rental_id`) REFERENCES `rentals` (`rental_id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `payments`
--

LOCK TABLES `payments` WRITE;
/*!40000 ALTER TABLE `payments` DISABLE KEYS */;
INSERT INTO `payments` VALUES (1,1,50000.00,'2024-01-10 00:00:00','Карта'),(2,2,12500.00,'2024-02-01 00:00:00','Наличные'),(3,3,55000.00,'2024-03-15 00:00:00','Карта'),(4,4,15000.00,'2024-04-10 00:00:00','Перевод'),(5,5,45000.00,'2024-05-20 00:00:00','Карта'),(6,6,14000.00,'2024-06-05 00:00:00','Наличные'),(7,7,28000.00,'2024-07-07 00:00:00','Перевод'),(8,8,19000.00,'2024-08-01 00:00:00','Карта'),(9,9,60000.00,'2024-09-20 00:00:00','Наличные'),(10,10,42000.00,'2024-10-25 00:00:00','Карта');
/*!40000 ALTER TABLE `payments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `rentals`
--

DROP TABLE IF EXISTS `rentals`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `rentals` (
  `rental_id` int NOT NULL AUTO_INCREMENT,
  `customer_id` int DEFAULT NULL,
  `car_id` int DEFAULT NULL,
  `rental_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `return_date` datetime DEFAULT NULL,
  `employee_id` int DEFAULT NULL,
  `total_amount` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`rental_id`),
  KEY `customer_id` (`customer_id`),
  KEY `car_id` (`car_id`),
  KEY `employee_id` (`employee_id`),
  CONSTRAINT `rentals_ibfk_1` FOREIGN KEY (`customer_id`) REFERENCES `customers` (`customer_id`),
  CONSTRAINT `rentals_ibfk_2` FOREIGN KEY (`car_id`) REFERENCES `cars` (`car_id`),
  CONSTRAINT `rentals_ibfk_3` FOREIGN KEY (`employee_id`) REFERENCES `employee` (`employee_id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rentals`
--

LOCK TABLES `rentals` WRITE;
/*!40000 ALTER TABLE `rentals` DISABLE KEYS */;
INSERT INTO `rentals` VALUES (1,1,2,'2024-01-10 00:00:00','2024-01-20 00:00:00',3,50000.00),(2,2,5,'2024-02-01 00:00:00','2024-02-05 00:00:00',4,12500.00),(3,3,7,'2024-03-15 00:00:00','2024-03-25 00:00:00',6,55000.00),(4,4,1,'2024-04-10 00:00:00','2024-04-15 00:00:00',2,15000.00),(5,5,3,'2024-05-20 00:00:00','2024-05-30 00:00:00',5,45000.00),(6,6,6,'2024-06-05 00:00:00','2024-06-12 00:00:00',1,14000.00),(7,7,8,'2024-07-07 00:00:00','2024-07-14 00:00:00',7,28000.00),(8,8,10,'2024-08-01 00:00:00','2024-08-05 00:00:00',9,19000.00),(9,9,4,'2024-09-20 00:00:00','2024-09-30 00:00:00',8,60000.00),(10,10,9,'2024-10-25 00:00:00','2024-11-05 00:00:00',10,42000.00);
/*!40000 ALTER TABLE `rentals` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `reviews`
--

DROP TABLE IF EXISTS `reviews`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reviews` (
  `review_id` int NOT NULL AUTO_INCREMENT,
  `customer_id` int DEFAULT NULL,
  `car_id` int DEFAULT NULL,
  `rating` int DEFAULT NULL,
  `comment` text,
  `review_date` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`review_id`),
  KEY `customer_id` (`customer_id`),
  KEY `car_id` (`car_id`),
  CONSTRAINT `reviews_ibfk_1` FOREIGN KEY (`customer_id`) REFERENCES `customers` (`customer_id`),
  CONSTRAINT `reviews_ibfk_2` FOREIGN KEY (`car_id`) REFERENCES `cars` (`car_id`),
  CONSTRAINT `reviews_chk_1` CHECK ((`rating` between 1 and 5))
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reviews`
--

LOCK TABLES `reviews` WRITE;
/*!40000 ALTER TABLE `reviews` DISABLE KEYS */;
INSERT INTO `reviews` VALUES (1,1,1,5,'Отличный автомобиль, очень комфортный и быстрый!','2025-01-05 10:30:00'),(2,2,2,4,'Хорошая машина, но немного неудобные сиденья.','2025-01-10 11:00:00'),(3,3,3,5,'Прекрасный опыт, рекомендую всем!','2025-02-01 12:15:00'),(4,4,4,3,'Машина в целом хорошая, но слишком жесткая подвеска.','2025-01-15 14:00:00'),(5,5,5,4,'Неплохая машина, но расход топлива высокий.','2025-01-20 15:30:00'),(6,6,6,5,'Очень нравится, отличный баланс между комфортом и мощностью.','2025-02-05 16:45:00'),(7,1,2,2,'Не понравился внешний вид и подвеска.','2025-02-08 09:30:00'),(8,2,3,4,'Машина хорошая, но иногда возникали проблемы с тормозами.','2025-01-25 10:00:00'),(9,3,4,5,'Очень довольна машиной, хорошая управляемость.','2025-02-10 11:30:00'),(10,4,5,3,'Машина надежная, но есть проблемы с кондиционером.','2025-02-12 13:00:00');
/*!40000 ALTER TABLE `reviews` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `role`
--

DROP TABLE IF EXISTS `role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `role` (
  `role_id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`role_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `role`
--

LOCK TABLES `role` WRITE;
/*!40000 ALTER TABLE `role` DISABLE KEYS */;
INSERT INTO `role` VALUES (1,'Администратор'),(2,'Менеджер');
/*!40000 ALTER TABLE `role` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-02-16 11:53:25
