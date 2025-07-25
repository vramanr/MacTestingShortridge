SET DEFAULT TO W:\foxapps\tables
USE orders_by_date___qa_code_del.dbf IN 1


SELECT co_id, co_name, s_addr1, s_addr2, s_city, s_state, s_zip, s_country FROM company WHERE co_id in (select co_id FROM orders_by_date___qa_code_del) INTO TABLE shippingaddresses