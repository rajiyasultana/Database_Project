<?php
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "unitybackend";

//Veriable submitted by user
$loginUser = $_POST["loginUser"];
$loginPass = $_POST["loginPass"];;

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$sql = "SELECT password FROM userstable WHERE username = '". $loginUser . "'";
$result = $conn->query($sql);

if ($result->num_rows > 0) {
  // output data of each row
  while($row = $result->fetch_assoc()) {
    if($row["password"] == $loginPass){
        echo "Login Success.";
        //Get user's data here.

        //Get player info

        //Get Inventory

        //Modify player data

        //update Inventory
    }
    else{
        echo "Wrong Password.";
    }
  }
} else {
  echo "Username does not exists";
}
$conn->close();

?>