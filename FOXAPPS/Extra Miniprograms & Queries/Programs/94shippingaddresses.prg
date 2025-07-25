SET DEFAULT TO W:\foxapps\tables


SELECT co_id, co_name, s_addr1, s_addr2, s_city, s_state, s_zip, s_country FROM company WHERE qa_req = "94" INTO TABLE 94shippingaddresses