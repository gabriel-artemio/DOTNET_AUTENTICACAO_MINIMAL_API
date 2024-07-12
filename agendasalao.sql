CREATE TABLE `horario` (
  `cd_horario` int NOT NULL AUTO_INCREMENT,
  `dt_atendimento` date DEFAULT NULL,
  `telefone_cliente` varchar(11) DEFAULT NULL,
  `nome_cliente` varchar(50) DEFAULT NULL,
  `cd_servico` int DEFAULT NULL,
  `cd_salao` int DEFAULT NULL,
  PRIMARY KEY (`cd_horario`),
  KEY `cd_servico` (`cd_servico`),
  KEY `cd_salao` (`cd_salao`),
  CONSTRAINT `horario_ibfk_1` FOREIGN KEY (`cd_servico`) REFERENCES `servico` (`cd_servico`),
  CONSTRAINT `horario_ibfk_2` FOREIGN KEY (`cd_salao`) REFERENCES `salao` (`cd_salao`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `salao` (
  `cd_salao` int NOT NULL AUTO_INCREMENT,
  `nm_salao` varchar(100) NOT NULL,
  `telefone_salao` varchar(11) DEFAULT NULL,
  `responsavel` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`cd_salao`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

CREATE TABLE `servico` (
  `cd_servico` int NOT NULL AUTO_INCREMENT,
  `nm_servico` varchar(100) NOT NULL,
  `valor` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`cd_servico`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;