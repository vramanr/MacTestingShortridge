CREATE VIEW v_model_serial AS;
SELECT Model_no.co_name, Model_no.co_id, Model_no.order_no,;
  Model_no.model_no, Model_no.serial_no, Model_no.cal_date,;
  Serialno.sn_notes;
 FROM  shortdata!model_no LEFT OUTER JOIN shortdata!serialno ;
   ON  Model_no.serial_no = Serialno.serial_no;
 WHERE Model_no.order_no = ?vorderno;
   AND Model_no.co_id = ?vcoid;
   AND DELETED() <> .T.;
 ORDER BY Model_no.model_no
 
CREATE VIEW v_ordetail AS; 
 SELECT Ordetail.part, Ordetail.quant, Ordetail.co_id, Ordetail.co_name,;
  Ordetail.order_no, Ordetail.entry_no, Ordetail.custom,;
  Ordetail.total_cost, Ordetail.nocharge, Partslst.descrption;
 FROM  shortdata!ordetail INNER JOIN shortdata!partslst ;
   ON  Ordetail.part = Partslst.part_no;
 WHERE Ordetail.part = Partslst.part_no;
   AND (Ordetail.co_id = ?VCOID;
   AND Ordetail.order_no = ?vorderno;
   AND DELETED() <> .T.);
 ORDER BY Ordetail.entry_no
 
 CREATE VIEW v_contact AS;
 SELECT *;
 FROM shortdata!contact;
 WHERE Contact.co_id = ?vCOID;
   AND DELETED() <> .T.;
 ORDER BY Contact.last_name