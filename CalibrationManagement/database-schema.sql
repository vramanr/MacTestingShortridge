
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";


CREATE TABLE cal_info (
    cal_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    cal_no VARCHAR(50) UNIQUE NOT NULL,
    co_id VARCHAR(20),
    order_no VARCHAR(50),
    serial_no VARCHAR(100),
    model_no VARCHAR(100),
    cal_date DATE,
    due_date DATE,
    cal_type VARCHAR(50),
    cal_tech VARCHAR(50),
    cal_status VARCHAR(20),
    temperature DECIMAL(8,2),
    humidity DECIMAL(5,2),
    pressure DECIMAL(8,2),
    notes TEXT,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    modified_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE cal_data (
    cal_data_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    cal_id UUID REFERENCES cal_info(cal_id),
    mode VARCHAR(50),
    set_point DECIMAL(15,6),
    actual_reading DECIMAL(15,6),
    deviation DECIMAL(15,6),
    percent_deviation DECIMAL(8,4),
    tolerance DECIMAL(15,6),
    pass_fail VARCHAR(10),
    units VARCHAR(20),
    sequence_no INTEGER,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE cal_standards (
    cal_std_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    cal_id UUID REFERENCES cal_info(cal_id),
    std_serial_no VARCHAR(100),
    std_model VARCHAR(100),
    std_cal_date DATE,
    std_due_date DATE,
    std_uncertainty DECIMAL(15,6),
    std_type VARCHAR(50),
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);


CREATE TABLE cal_setup (
    cal_setup_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    cal_type VARCHAR(50),
    mode VARCHAR(50),
    cal_std VARCHAR(100),
    cal_std2 VARCHAR(100),
    sort_order INTEGER,
    active BOOLEAN DEFAULT TRUE,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE cal_types (
    cal_type_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    cal_type VARCHAR(50) UNIQUE NOT NULL,
    description TEXT,
    active BOOLEAN DEFAULT TRUE,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE cal_techs (
    cal_tech_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    tech_name VARCHAR(100) NOT NULL,
    tech_initials VARCHAR(10),
    active BOOLEAN DEFAULT TRUE,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE set_point (
    set_point_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    cal_type VARCHAR(50),
    set_value DECIMAL(15,6),
    units VARCHAR(20),
    sequence_no INTEGER,
    active BOOLEAN DEFAULT TRUE,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE tolerances (
    tolerance_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    cal_type VARCHAR(50),
    range_min DECIMAL(15,6),
    range_max DECIMAL(15,6),
    tolerance_value DECIMAL(15,6),
    tolerance_type VARCHAR(20), -- 'ABSOLUTE' or 'PERCENT'
    units VARCHAR(20),
    active BOOLEAN DEFAULT TRUE,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);


CREATE TABLE company (
    company_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    co_id VARCHAR(20) UNIQUE NOT NULL,
    co_name VARCHAR(200),
    address1 VARCHAR(100),
    address2 VARCHAR(100),
    city VARCHAR(50),
    state VARCHAR(10),
    zip VARCHAR(20),
    country VARCHAR(50),
    phone VARCHAR(30),
    fax VARCHAR(30),
    email VARCHAR(100),
    website VARCHAR(200),
    tax_id VARCHAR(50),
    terms VARCHAR(50),
    discount_code VARCHAR(20),
    credit_limit DECIMAL(12,2),
    balance DECIMAL(12,2),
    active BOOLEAN DEFAULT TRUE,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    modified_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE contact (
    contact_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    co_id VARCHAR(20) REFERENCES company(co_id),
    first_name VARCHAR(50),
    last_name VARCHAR(50),
    title VARCHAR(100),
    phone VARCHAR(30),
    email VARCHAR(100),
    primary_contact BOOLEAN DEFAULT FALSE,
    active BOOLEAN DEFAULT TRUE,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE ordrstat (
    order_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    order_no VARCHAR(50) UNIQUE NOT NULL,
    co_id VARCHAR(20) REFERENCES company(co_id),
    order_date DATE,
    due_date DATE,
    status VARCHAR(50),
    priority VARCHAR(20),
    notes TEXT,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    modified_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE ordetail (
    order_detail_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    order_no VARCHAR(50) REFERENCES ordrstat(order_no),
    line_no INTEGER,
    part_no VARCHAR(100),
    description TEXT,
    quantity INTEGER,
    unit_price DECIMAL(12,2),
    extended_price DECIMAL(12,2),
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE model_no (
    model_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    serial_no VARCHAR(100) UNIQUE NOT NULL,
    model_no VARCHAR(100),
    co_id VARCHAR(20) REFERENCES company(co_id),
    co_name VARCHAR(200),
    order_no VARCHAR(50),
    cal_date DATE,
    due_date DATE,
    cal_type VARCHAR(50),
    status VARCHAR(50),
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    modified_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE model_no_cal (
    model_cal_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    serial_no VARCHAR(100),
    model_no VARCHAR(100),
    co_id VARCHAR(20),
    cal_date DATE,
    cal_type VARCHAR(50),
    active BOOLEAN DEFAULT TRUE,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);


CREATE TABLE partslst (
    part_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    part_no VARCHAR(100) UNIQUE NOT NULL,
    description TEXT,
    unit_price DECIMAL(12,2),
    category VARCHAR(50),
    active BOOLEAN DEFAULT TRUE,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE partsize (
    partsize_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    part_no VARCHAR(100),
    size_code VARCHAR(20),
    description VARCHAR(200),
    active BOOLEAN DEFAULT TRUE,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE users (
    user_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    username VARCHAR(50) UNIQUE NOT NULL,
    password_hash VARCHAR(255),
    first_name VARCHAR(50),
    last_name VARCHAR(50),
    email VARCHAR(100),
    role VARCHAR(50),
    active BOOLEAN DEFAULT TRUE,
    last_login TIMESTAMP,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE ship_via (
    ship_via_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    ship_code VARCHAR(20) UNIQUE NOT NULL,
    description VARCHAR(100),
    active BOOLEAN DEFAULT TRUE,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);


CREATE TABLE invoice (
    invoice_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    invoice_no VARCHAR(50) UNIQUE NOT NULL,
    co_id VARCHAR(20) REFERENCES company(co_id),
    order_no VARCHAR(50),
    inv_date DATE,
    due_date DATE,
    subtotal DECIMAL(12,2),
    tax_amount DECIMAL(12,2),
    total_amount DECIMAL(12,2),
    paid_amount DECIMAL(12,2),
    balance DECIMAL(12,2),
    status VARCHAR(50),
    qa_code VARCHAR(20),
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE pay_dtl (
    payment_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    invoice_no VARCHAR(50) REFERENCES invoice(invoice_no),
    payment_date DATE,
    payment_amount DECIMAL(12,2),
    payment_method VARCHAR(50),
    check_no VARCHAR(50),
    notes TEXT,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE amount (
    amount_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    invoice_no VARCHAR(50),
    amount_type VARCHAR(50),
    amount_value DECIMAL(12,2),
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);


CREATE TABLE cal_no (
    cal_no_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    next_cal_no INTEGER DEFAULT 1,
    prefix VARCHAR(10),
    suffix VARCHAR(10),
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE quote_no (
    quote_no_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    next_quote_no INTEGER DEFAULT 1,
    prefix VARCHAR(10),
    suffix VARCHAR(10),
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE order_no (
    order_no_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    next_order_no INTEGER DEFAULT 1,
    prefix VARCHAR(10),
    suffix VARCHAR(10),
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE p_ord_no (
    p_ord_no_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    next_po_no INTEGER DEFAULT 1,
    prefix VARCHAR(10),
    suffix VARCHAR(10),
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE conumber (
    conumber_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    next_co_no INTEGER DEFAULT 1,
    prefix VARCHAR(10),
    suffix VARCHAR(10),
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE custom_i (
    custom_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    next_custom_no INTEGER DEFAULT 1,
    prefix VARCHAR(10),
    suffix VARCHAR(10),
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);


CREATE TABLE quote_header (
    quote_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    quote_no VARCHAR(50) UNIQUE NOT NULL,
    co_id VARCHAR(20) REFERENCES company(co_id),
    quote_date DATE,
    expiry_date DATE,
    status VARCHAR(50),
    subtotal DECIMAL(12,2),
    tax_amount DECIMAL(12,2),
    total_amount DECIMAL(12,2),
    notes TEXT,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE quote_detail (
    quote_detail_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    quote_no VARCHAR(50) REFERENCES quote_header(quote_no),
    line_no INTEGER,
    part_no VARCHAR(100),
    description TEXT,
    quantity INTEGER,
    unit_price DECIMAL(12,2),
    extended_price DECIMAL(12,2),
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE quote_comment (
    comment_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    quote_no VARCHAR(50) REFERENCES quote_header(quote_no),
    comment_text TEXT,
    comment_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    user_id UUID REFERENCES users(user_id),
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE shipping (
    shipping_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    order_no VARCHAR(50),
    ship_date DATE,
    ship_via VARCHAR(50),
    tracking_no VARCHAR(100),
    ship_cost DECIMAL(12,2),
    weight DECIMAL(8,2),
    notes TEXT,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE serialno (
    serial_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    serial_no VARCHAR(100) UNIQUE NOT NULL,
    part_no VARCHAR(100),
    status VARCHAR(50),
    location VARCHAR(100),
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE overdue (
    overdue_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    item_type VARCHAR(50), -- 'CALIBRATION', 'INVOICE', etc.
    item_id VARCHAR(50),
    due_date DATE,
    days_overdue INTEGER,
    status VARCHAR(50),
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE misc_chk (
    misc_chk_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    check_type VARCHAR(50),
    check_value VARCHAR(200),
    description TEXT,
    active BOOLEAN DEFAULT TRUE,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);


CREATE TABLE precheck (
    precheck_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    order_no VARCHAR(50),
    serial_no VARCHAR(100),
    model_no VARCHAR(100),
    co_id VARCHAR(20),
    problem_description TEXT,
    condition_received VARCHAR(200),
    accessories TEXT,
    repair_status VARCHAR(50),
    tech_assigned VARCHAR(50),
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE labels (
    label_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    label_type VARCHAR(50),
    item_id VARCHAR(50),
    label_data TEXT,
    print_count INTEGER DEFAULT 0,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE pc_misc (
    pc_misc_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    misc_type VARCHAR(50),
    misc_value VARCHAR(200),
    description TEXT,
    active BOOLEAN DEFAULT TRUE,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);


CREATE TABLE app_setup (
    setup_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    setting_name VARCHAR(100) UNIQUE NOT NULL,
    setting_value TEXT,
    setting_type VARCHAR(50), -- 'STRING', 'INTEGER', 'BOOLEAN', 'DECIMAL'
    description TEXT,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    modified_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE tables (
    table_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    table_name VARCHAR(100) UNIQUE NOT NULL,
    description TEXT,
    record_count INTEGER DEFAULT 0,
    last_updated TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    active BOOLEAN DEFAULT TRUE
);


CREATE TABLE acct_code (
    acct_code_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    acct_code VARCHAR(20) UNIQUE NOT NULL,
    description VARCHAR(200),
    acct_type VARCHAR(50),
    active BOOLEAN DEFAULT TRUE,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE disc_code (
    disc_code_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    disc_code VARCHAR(20) UNIQUE NOT NULL,
    description VARCHAR(200),
    discount_percent DECIMAL(5,2),
    active BOOLEAN DEFAULT TRUE,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE terms_code (
    terms_code_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    terms_code VARCHAR(20) UNIQUE NOT NULL,
    description VARCHAR(200),
    net_days INTEGER,
    discount_days INTEGER,
    discount_percent DECIMAL(5,2),
    active BOOLEAN DEFAULT TRUE,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE subacct_code (
    subacct_code_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    subacct_code VARCHAR(20) UNIQUE NOT NULL,
    description VARCHAR(200),
    parent_acct VARCHAR(20),
    active BOOLEAN DEFAULT TRUE,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE tax_code (
    tax_code_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    tax_code VARCHAR(20) UNIQUE NOT NULL,
    description VARCHAR(200),
    tax_rate DECIMAL(8,4),
    active BOOLEAN DEFAULT TRUE,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE qa_code (
    qa_code_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    qa_code VARCHAR(20) UNIQUE NOT NULL,
    description VARCHAR(200),
    active BOOLEAN DEFAULT TRUE,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);


CREATE TABLE box_size (
    box_size_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    size_code VARCHAR(20) UNIQUE NOT NULL,
    description VARCHAR(200),
    length_inches DECIMAL(8,2),
    width_inches DECIMAL(8,2),
    height_inches DECIMAL(8,2),
    weight_limit DECIMAL(8,2),
    active BOOLEAN DEFAULT TRUE,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE modes (
    mode_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    mode_code VARCHAR(20) UNIQUE NOT NULL,
    description VARCHAR(200),
    cal_type VARCHAR(50),
    active BOOLEAN DEFAULT TRUE,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE procedures (
    procedure_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    procedure_code VARCHAR(50) UNIQUE NOT NULL,
    procedure_name VARCHAR(200),
    description TEXT,
    cal_type VARCHAR(50),
    version VARCHAR(20),
    active BOOLEAN DEFAULT TRUE,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE uncertainties (
    uncertainty_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    cal_type VARCHAR(50),
    measurement_type VARCHAR(50),
    uncertainty_value DECIMAL(15,6),
    uncertainty_units VARCHAR(20),
    confidence_level DECIMAL(5,2),
    active BOOLEAN DEFAULT TRUE,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE documents (
    document_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    document_name VARCHAR(200),
    document_type VARCHAR(50),
    file_path VARCHAR(500),
    file_size INTEGER,
    mime_type VARCHAR(100),
    related_table VARCHAR(100),
    related_id UUID,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);

CREATE TABLE report_memo (
    memo_id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    report_type VARCHAR(50),
    memo_text TEXT,
    created_by UUID REFERENCES users(user_id),
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    deleted BOOLEAN DEFAULT FALSE
);


CREATE INDEX idx_cal_info_cal_no ON cal_info(cal_no);
CREATE INDEX idx_cal_info_co_id ON cal_info(co_id);
CREATE INDEX idx_cal_info_serial_no ON cal_info(serial_no);
CREATE INDEX idx_cal_info_cal_date ON cal_info(cal_date);
CREATE INDEX idx_cal_info_due_date ON cal_info(due_date);
CREATE INDEX idx_cal_info_cal_type ON cal_info(cal_type);

CREATE INDEX idx_cal_data_cal_id ON cal_data(cal_id);
CREATE INDEX idx_cal_data_mode ON cal_data(mode);

CREATE INDEX idx_cal_standards_cal_id ON cal_standards(cal_id);
CREATE INDEX idx_cal_standards_std_serial ON cal_standards(std_serial_no);

CREATE INDEX idx_company_co_id ON company(co_id);
CREATE INDEX idx_company_co_name ON company(co_name);

CREATE INDEX idx_contact_co_id ON contact(co_id);

CREATE INDEX idx_ordrstat_order_no ON ordrstat(order_no);
CREATE INDEX idx_ordrstat_co_id ON ordrstat(co_id);
CREATE INDEX idx_ordrstat_order_date ON ordrstat(order_date);

CREATE INDEX idx_model_no_serial_no ON model_no(serial_no);
CREATE INDEX idx_model_no_co_id ON model_no(co_id);
CREATE INDEX idx_model_no_cal_date ON model_no(cal_date);

CREATE INDEX idx_invoice_invoice_no ON invoice(invoice_no);
CREATE INDEX idx_invoice_co_id ON invoice(co_id);
CREATE INDEX idx_invoice_inv_date ON invoice(inv_date);


CREATE OR REPLACE FUNCTION update_modified_date()
RETURNS TRIGGER AS $$
BEGIN
    NEW.modified_date = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER tr_cal_info_modified
    BEFORE UPDATE ON cal_info
    FOR EACH ROW
    EXECUTE FUNCTION update_modified_date();

CREATE TRIGGER tr_company_modified
    BEFORE UPDATE ON company
    FOR EACH ROW
    EXECUTE FUNCTION update_modified_date();

CREATE TRIGGER tr_ordrstat_modified
    BEFORE UPDATE ON ordrstat
    FOR EACH ROW
    EXECUTE FUNCTION update_modified_date();

CREATE TRIGGER tr_model_no_modified
    BEFORE UPDATE ON model_no
    FOR EACH ROW
    EXECUTE FUNCTION update_modified_date();


INSERT INTO app_setup (setting_name, setting_value, setting_type, description) VALUES
('company_name', 'Shortridge Instruments', 'STRING', 'Company name for reports'),
('default_cal_tech', 'ADMIN', 'STRING', 'Default calibration technician'),
('cal_cert_template', 'standard', 'STRING', 'Default calibration certificate template'),
('auto_generate_cal_no', 'true', 'BOOLEAN', 'Automatically generate calibration numbers'),
('cal_no_prefix', 'CAL-', 'STRING', 'Prefix for calibration numbers'),
('default_tolerance_type', 'PERCENT', 'STRING', 'Default tolerance type for new calibrations');

INSERT INTO cal_techs (tech_name, tech_initials, active) VALUES
('Administrator', 'ADMIN', true);

INSERT INTO users (username, first_name, last_name, email, role, active) VALUES
('admin', 'System', 'Administrator', 'admin@shortridge.com', 'ADMIN', true);

INSERT INTO cal_no (next_cal_no, prefix) VALUES (1, 'CAL-');
INSERT INTO quote_no (next_quote_no, prefix) VALUES (1, 'Q-');
INSERT INTO order_no (next_order_no, prefix) VALUES (1, 'O-');
INSERT INTO p_ord_no (next_po_no, prefix) VALUES (1, 'PO-');
INSERT INTO conumber (next_co_no, prefix) VALUES (1, 'C-');


CREATE VIEW v_active_calibrations AS
SELECT 
    ci.cal_id,
    ci.cal_no,
    ci.serial_no,
    ci.model_no,
    c.co_name,
    ci.cal_date,
    ci.due_date,
    ci.cal_type,
    ci.cal_tech,
    ci.cal_status,
    CASE 
        WHEN ci.due_date < CURRENT_DATE THEN 'OVERDUE'
        WHEN ci.due_date <= CURRENT_DATE + INTERVAL '30 days' THEN 'DUE_SOON'
        ELSE 'CURRENT'
    END as due_status
FROM cal_info ci
LEFT JOIN company c ON ci.co_id = c.co_id
WHERE ci.deleted = false
    AND ci.cal_status IN ('ACTIVE', 'COMPLETED');

CREATE VIEW v_overdue_calibrations AS
SELECT 
    ci.cal_id,
    ci.cal_no,
    ci.serial_no,
    ci.model_no,
    c.co_name,
    ci.due_date,
    CURRENT_DATE - ci.due_date as days_overdue
FROM cal_info ci
LEFT JOIN company c ON ci.co_id = c.co_id
WHERE ci.deleted = false
    AND ci.due_date < CURRENT_DATE
    AND ci.cal_status = 'ACTIVE';

CREATE VIEW v_calibration_summary AS
SELECT 
    ci.cal_id,
    ci.cal_no,
    ci.serial_no,
    ci.model_no,
    c.co_name,
    ci.cal_date,
    ci.due_date,
    ci.cal_type,
    COUNT(cd.cal_data_id) as measurement_count,
    COUNT(CASE WHEN cd.pass_fail = 'PASS' THEN 1 END) as pass_count,
    COUNT(CASE WHEN cd.pass_fail = 'FAIL' THEN 1 END) as fail_count
FROM cal_info ci
LEFT JOIN company c ON ci.co_id = c.co_id
LEFT JOIN cal_data cd ON ci.cal_id = cd.cal_id AND cd.deleted = false
WHERE ci.deleted = false
GROUP BY ci.cal_id, ci.cal_no, ci.serial_no, ci.model_no, c.co_name, 
         ci.cal_date, ci.due_date, ci.cal_type;

CREATE VIEW v_company_summary AS
SELECT 
    c.company_id,
    c.co_id,
    c.co_name,
    c.city,
    c.state,
    c.phone,
    c.email,
    COUNT(DISTINCT ci.cal_id) as total_calibrations,
    COUNT(DISTINCT CASE WHEN ci.cal_status = 'ACTIVE' THEN ci.cal_id END) as active_calibrations,
    MAX(ci.cal_date) as last_calibration_date
FROM company c
LEFT JOIN cal_info ci ON c.co_id = ci.co_id AND ci.deleted = false
WHERE c.deleted = false AND c.active = true
GROUP BY c.company_id, c.co_id, c.co_name, c.city, c.state, c.phone, c.email;


COMMENT ON TABLE cal_info IS 'Main calibration records containing header information';
COMMENT ON TABLE cal_data IS 'Detailed measurement data for each calibration point';
COMMENT ON TABLE cal_standards IS 'Reference standards used in calibrations';
COMMENT ON TABLE company IS 'Customer company information';
COMMENT ON TABLE users IS 'System users and technicians';
