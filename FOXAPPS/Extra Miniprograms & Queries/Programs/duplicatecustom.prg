OPEN DATABASE W:\foxapps\shortdata

SELECT a.order_no, a.custom, b.order_no, a.custom ;
FROM ordetail a, ordetail b ;
WHERE a.custom = b.custom and a.custom <> " " AND a.order_no <> b.order_no;
ORDER BY a.custom