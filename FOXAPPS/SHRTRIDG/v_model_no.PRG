SELECT Model_no.model_no_id, Model_no.co_id, Model_no.co_name,;
  Model_no.order_no, Model_no.model_no, Model_no.serial_no,;
  Model_no.cal_date, Serialno.serial_id, Serialno.sn_notes;
 FROM ;
     shortdata!model_no ;
    LEFT OUTER JOIN shortdata!serialno ;
   ON  Model_no.co_id = Serialno.co_id;
   AND  Model_no.serial_no = Serialno.serial_no;
 WHERE  Model_no.co_id = ( ?vCOID );
   AND  (  Model_no.order_no = ( ?vORDERNO );
   AND  DELETED() = ( .F. ) )

DBSetProp(ThisView,"View","SendUpdates",.T.)
DBSetProp(ThisView,"View","BatchUpdateCount",1)
DBSetProp(ThisView,"View","CompareMemo",.T.)
DBSetProp(ThisView,"View","FetchAsNeeded",.F.)
DBSetProp(ThisView,"View","FetchMemo",.F.)
DBSetProp(ThisView,"View","FetchSize",100)
DBSetProp(ThisView,"View","MaxRecords",-1)
DBSetProp(ThisView,"View","Prepared",.F.)
DBSetProp(ThisView,"View","UpdateType",1)
DBSetProp(ThisView,"View","UseMemoSize",255)
DBSetProp(ThisView,"View","Tables","shortdata!model_no,shortdata!serialno")
DBSetProp(ThisView,"View","WhereType",1)

DBSetProp(ThisView+".model_no_id","Field","DataType","I")
DBSetProp(ThisView+".model_no_id","Field","UpdateName","shortdata!model_no.model_no_id")
DBSetProp(ThisView+".model_no_id","Field","KeyField",.T.)
DBSetProp(ThisView+".model_no_id","Field","Updatable",.F.)

DBSetProp(ThisView+".co_id","Field","DataType","C(6)")
DBSetProp(ThisView+".co_id","Field","UpdateName","shortdata!model_no.co_id")
DBSetProp(ThisView+".co_id","Field","KeyField",.F.)
DBSetProp(ThisView+".co_id","Field","Updatable",.T.)

DBSetProp(ThisView+".co_name","Field","DataType","C(20)")
DBSetProp(ThisView+".co_name","Field","UpdateName","shortdata!model_no.co_name")
DBSetProp(ThisView+".co_name","Field","KeyField",.F.)
DBSetProp(ThisView+".co_name","Field","Updatable",.T.)

DBSetProp(ThisView+".order_no","Field","DataType","C(8)")
DBSetProp(ThisView+".order_no","Field","UpdateName","shortdata!model_no.order_no")
DBSetProp(ThisView+".order_no","Field","KeyField",.F.)
DBSetProp(ThisView+".order_no","Field","Updatable",.T.)

DBSetProp(ThisView+".model_no","Field","DataType","C(15)")
DBSetProp(ThisView+".model_no","Field","UpdateName","shortdata!model_no.model_no")
DBSetProp(ThisView+".model_no","Field","KeyField",.F.)
DBSetProp(ThisView+".model_no","Field","Updatable",.T.)

DBSetProp(ThisView+".serial_no","Field","DataType","C(12)")
DBSetProp(ThisView+".serial_no","Field","UpdateName","shortdata!model_no.serial_no")
DBSetProp(ThisView+".serial_no","Field","KeyField",.F.)
DBSetProp(ThisView+".serial_no","Field","Updatable",.T.)

DBSetProp(ThisView+".cal_date","Field","DataType","D")
DBSetProp(ThisView+".cal_date","Field","UpdateName","shortdata!model_no.cal_date")
DBSetProp(ThisView+".cal_date","Field","KeyField",.F.)
DBSetProp(ThisView+".cal_date","Field","Updatable",.T.)

DBSetProp(ThisView+".serial_id","Field","DataType","I")
DBSetProp(ThisView+".serial_id","Field","UpdateName","shortdata!serialno.serial_id")
DBSetProp(ThisView+".serial_id","Field","KeyField",.T.)
DBSetProp(ThisView+".serial_id","Field","Updatable",.F.)

DBSetProp(ThisView+".sn_notes","Field","DataType","C(200)")
DBSetProp(ThisView+".sn_notes","Field","UpdateName","shortdata!serialno.sn_notes")
DBSetProp(ThisView+".sn_notes","Field","KeyField",.F.)
DBSetProp(ThisView+".sn_notes","Field","Updatable",.F.)

