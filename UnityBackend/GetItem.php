<?php
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "unitybackend";

//Veriable submitted by user
//$loginUser = $_POST["loginUser"];
//$loginPass = $_POST["loginPass"];
$itemID = $_POST["itemID"];

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$sql = "SELECT Name, Description, Price FROM items WHERE ID = '". $itemID . "'";
$result = $conn->query($sql);

if ($result->num_rows > 0) 
{
    // output data of each row
    $rows = array();
    while($row = $result->fetch_assoc()) 
    {
      $rows[] = $row;
    }
    echo json_encode($rows);
} 
else 
{
  echo "0";
}
$conn->close();

?>