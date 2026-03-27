CREATE TABLE veiculos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    placa VARCHAR(10) UNIQUE,
    modelo VARCHAR(50),
    cor VARCHAR(30)
);

CREATE TABLE vagas (
    id INT AUTO_INCREMENT PRIMARY KEY,
    numero INT,
    status VARCHAR(20) DEFAULT 'LIVRE' -- LIVRE / OCUPADA
);

CREATE TABLE movimentacoes (
    id INT AUTO_INCREMENT PRIMARY KEY,
    veiculo_id INT,
    vaga_id INT,
    data_entrada DATETIME DEFAULT CURRENT_TIMESTAMP,
    data_saida DATETIME NULL,
    valor_pago DECIMAL(10,2) DEFAULT 0,
    status VARCHAR(20) DEFAULT 'ABERTA',

    FOREIGN KEY (veiculo_id) REFERENCES veiculos(id),
    FOREIGN KEY (vaga_id) REFERENCES vagas(id)
);

-- TRIGGER AO OCUPAR VAGA
DELIMITER $$

CREATE TRIGGER trg_ocupar_vaga
AFTER INSERT ON movimentacoes
FOR EACH ROW
BEGIN
    UPDATE vagas
    SET status = 'OCUPADA'
    WHERE id = NEW.vaga_id;
END$$

DELIMITER ;

-- TRIGGER AO LIBERAR VAGA
DELIMITER $$

CREATE TRIGGER trg_liberar_vaga
AFTER UPDATE ON movimentacoes
FOR EACH ROW
BEGIN
    IF NEW.status = 'FINALIZADA' AND OLD.status <> 'FINALIZADA' THEN
        UPDATE vagas
        SET status = 'LIVRE'
        WHERE id = NEW.vaga_id;
    END IF;
END$$

DELIMITER ;

-- PROCEDURE AO REGISTRAR ENTRADA
DELIMITER $$

CREATE PROCEDURE sp_registrar_entrada (
    IN p_placa VARCHAR(10),
    IN p_vaga_id INT
)
BEGIN
    DECLARE v_veiculo_id INT;
    DECLARE v_status_vaga VARCHAR(20);

    -- Verifica vaga
    SELECT status INTO v_status_vaga
    FROM vagas
    WHERE id = p_vaga_id;

    IF v_status_vaga = 'OCUPADA' THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Vaga já está ocupada';
    END IF;

    -- Busca veículo
    SELECT id INTO v_veiculo_id
    FROM veiculos
    WHERE placa = p_placa;

    IF v_veiculo_id IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Veículo não cadastrado';
    END IF;

    -- Cria movimentação
    INSERT INTO movimentacoes (veiculo_id, vaga_id)
    VALUES (v_veiculo_id, p_vaga_id);

END$$

DELIMITER ;

-- PROCEDURE AO REGISTRAR SAIDA
DELIMITER $$

CREATE PROCEDURE sp_registrar_saida (
    IN p_movimentacao_id INT
)
BEGIN
    DECLARE v_data_entrada DATETIME;
    DECLARE v_valor DECIMAL(10,2);

    -- Busca entrada
    SELECT data_entrada INTO v_data_entrada
    FROM movimentacoes
    WHERE id = p_movimentacao_id;

    -- Cálculo: R$10 por hora
    SET v_valor = TIMESTAMPDIFF(HOUR, v_data_entrada, NOW()) * 10;

    -- Atualiza saída
    UPDATE movimentacoes
    SET 
        data_saida = NOW(),
        valor_pago = v_valor,
        status = 'FINALIZADA'
    WHERE id = p_movimentacao_id;

END$$

DELIMITER ;