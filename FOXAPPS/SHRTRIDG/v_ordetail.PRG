SELECT Ordetail.part, Ordetail.quant, Ordetail.co_id, Ordetail.co_name,;
  Ordetail.order_no, Ordetail.entry_no, Ordetail.custom,;
  Ordetail.total_cost, Ordetail.nocharge, Ordetail.ordetail_id,;
  Partslst.descrption, Partslst.price_each, Custom.co_id AS c_co_id,;
  Custom.order_no AS c_order_no, Custom.custom AS c_custom,;
  Custom.part_no, Custom.descript, Custom.cost;
 FROM ;
     shortdata!ordetail ;
    INNER JOIN shortdata!partslst ;
   ON  Ordetail.part = Partslst.part_no ;
    LEFT OUTER JOIN shortdata!custom ;
   ON  Ordetail.custom = Custom.custom;
 WHERE  Ordetail.part = Partslst.part_no;
   AND  ( (  Ordetail.co_id = ( ?VCOID );
   AND  Ordetail.order_no = ( ?vorderno ) );
   AND  DELETED() <> ( .T. ) );
 ORDER BY Ordetail.entry_no

DBSetProp(ThisView,"View","SendUpdates",.T.)
DBSetProp(ThisView,"View","BatchUpdateCount",1)
DBSetProp(ThisView,"View","CompareMemo",.T.)
DBSetProp(ThisView,"View","FetchAsNeeded",.F.)
DBSetProp(ThisView,"View","FetchMemo",.T.)
DBSetProp(ThisView,"View","FetchSize",100)
DBSetProp(ThisView,"View","MaxRecords",-1)
DBSetProp(ThisView,"View","Prepared",.F.)
DBSetProp(ThisView,"View","UpdateType",1)
DBSetProp(ThisView,"View","UseMemoSize",255)
DBSetProp(ThisView,"View","Tables","shortdata!ordetail")
DBSetProp(ThisView,"View","WhereType",1)

DBSetProp(ThisView+".part","Field","DataType","C(13)")
DBSetProp(ThisView+".part","Field","UpdateName","shortdata!ordetail.part")
DBSetProp(ThisView+".part","Field","KeyField",.F.)
DBSetProp(ThisView+".part","Field","Updatable",.T.)

DBSetProp(ThisView+".quant","Field","DataType","N(7,2)")
DBSetProp(ThisView+".quant","Field","UpdateName","shortdata!ordetail.quant")
DBSetProp(ThisView+".quant","Field","KeyField",.F.)
DBSetProp(ThisView+".quant","Field","Updatable",.T.)

DBSetProp(ThisView+".co_id","Field","DataType","C(6)")
DBSetProp(ThisView+".co_id","Field","UpdateName","shortdata!ordetail.co_id")
DBSetProp(ThisView+".co_id","Field","KeyField",.F.)
DBSetProp(ThisView+".co_id","Field","Updatable",.T.)

DBSetProp(ThisView+".co_name","Field","DataType","C(20)")
DBSetProp(ThisView+".co_name","Field","UpdateName","shortdata!ordetail.co_name")
DBSetProp(ThisView+".co_name","Field","KeyField",.F.)
DBSetProp(ThisView+".co_name","Field","Updatable",.T.)

DBSetProp(ThisView+".order_no","Field","DataType","C(8)")
DBSetProp(ThisView+".order_no","Field","UpdateName","shortdata!ordetail.order_no")
DBSetProp(ThisView+".order_no","Field","KeyField",.F.)
DBSetProp(ThisView+".order_no","Field","Updatable",.T.)

DBSetProp(ThisView+".entry_no","Field","DataType","N(2)")
DBSetProp(ThisView+".entry_no","Field","UpdateName","shortdata!ordetail.entry_no")
DBSetProp(ThisView+".entry_no","Field","KeyField",.F.)
DBSetProp(ThisView+".entry_no","Field","Updatable",.T.)

DBSetProp(ThisView+".custom","Field","DataType","C(7)")
DBSetProp(ThisView+".custom","Field","UpdateName","shortdata!ordetail.custom")
DBSetProp(ThisView+".custom","Field","KeyField",.F.)
DBSetProp(ThisView+".custom","Field","Updatable",.T.)

DBSetProp(ThisView+".total_cost","Field","DataType","N(10,2)")
DBSetProp(ThisView+".total_cost","Field","UpdateName","shortdata!ordetail.total_cost")
DBSetProp(ThisView+".total_cost","Field","KeyField",.F.)
DBSetProp(ThisView+".total_cost","Field","Updatable",.T.)

DBSetProp(ThisView+".nocharge","Field","DataType","C(2)")
DBSetProp(ThisView+".nocharge","Field","UpdateName","shortdata!ordetail.nocharge")
DBSetProp(ThisView+".nocharge","Field","KeyField",.F.)
DBSetProp(ThisView+".nocharge","Field","Updatable",.T.)

DBSetProp(ThisView+".ordetail_id","Field","DataType","I")
DBSetProp(ThisView+".ordetail_id","Field","UpdateName","shortdata!ordetail.ordetail_id")
DBSetProp(ThisView+".ordetail_id","Field","KeyField",.T.)
DBSetProp(ThisView+".ordetail_id","Field","Updatable",.F.)

DBSetProp(ThisView+".descrption","Field","DataType","C(68)")
DBSetProp(ThisView+".descrption","Field","UpdateName","shortdata!partslst.descrption")
DBSetProp(ThisView+".descrption","Field","KeyField",.F.)
DBSetProp(ThisView+".descrption","Field","Updatable",.F.)

DBSetProp(ThisView+".price_each","Field","DataType","N(7,2)")
DBSetProp(ThisView+".price_each","Field","UpdateName","shortdata!partslst.price_each")
DBSetProp(ThisView+".price_each","Field","KeyField",.F.)
DBSetProp(ThisView+".price_each","Field","Updatable",.F.)

DBSetProp(ThisView+".c_co_id","Field","DataType","C(6)")
DBSetProp(ThisView+".c_co_id","Field","UpdateName","shortdata!custom.co_id")
DBSetProp(ThisView+".c_co_id","Field","KeyField",.F.)
DBSetProp(ThisView+".c_co_id","Field","Updatable",.F.)

DBSetProp(ThisView+".c_order_no","Field","DataType","C(10)")
DBSetProp(ThisView+".c_order_no","Field","UpdateName","shortdata!custom.order_no")
DBSetProp(ThisView+".c_order_no","Field","KeyField",.F.)
DBSetProp(ThisView+".c_order_no","Field","Updatable",.F.)

DBSetProp(ThisView+".c_custom","Field","DataType","C(7)")
DBSetProp(ThisView+".c_custom","Field","UpdateName","shortdata!custom.custom")
DBSetProp(ThisView+".c_custom","Field","KeyField",.T.)
DBSetProp(ThisView+".c_custom","Field","Updatable",.F.)

DBSetProp(ThisView+".part_no","Field","DataType","C(13)")
DBSetProp(ThisView+".part_no","Field","UpdateName","shortdata!custom.part_no")
DBSetProp(ThisView+".part_no","Field","KeyField",.F.)
DBSetProp(ThisView+".part_no","Field","Updatable",.F.)

DBSetProp(ThisView+".descript","Field","DataType","C(50)")
DBSetProp(ThisView+".descript","Field","UpdateName","shortdata!custom.descript")
DBSetProp(ThisView+".descript","Field","KeyField",.F.)
DBSetProp(ThisView+".descript","Field","Updatable",.F.)

DBSetProp(ThisView+".cost","Field","DataType","N(10,2)")
DBSetProp(ThisView+".cost","Field","UpdateName","shortdata!custom.cost")
DBSetProp(ThisView+".cost","Field","KeyField",.F.)
DBSetProp(ThisView+".cost","Field","Updatable",.F.)

