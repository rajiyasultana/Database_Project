<?php
//test file for userid check.
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "unitybackend";

print_r($_POST);

//user submited veriables
$userID = $_GET["userID"];

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$sql = "SELECT itemID FROM usersitems WHERE userID = '". $userID . "'";
$result = $conn->query($sql);

if ($result->num_rows > 0) {
  // output data of each row
  $rows = array();
  while($row = $result->fetch_assoc()) {
    $rows[] = $row;
  }
  echo json_encode($rows);
} else {
  echo "0";
}
$conn->close();

?>