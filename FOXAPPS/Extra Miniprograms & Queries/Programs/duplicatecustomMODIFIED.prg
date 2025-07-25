OPEN DATABASE w:\foxapps\shortdata

SELECT a.order_no, a.custom, A.DESCRIPT, b.order_no, a.custom, ;
FROM custom a, custom b ;
WHERE a.custom = b.custom and a.custom <> " " AND a.order_no <> b.order_no;
ORDER BY a.ORDER_NO