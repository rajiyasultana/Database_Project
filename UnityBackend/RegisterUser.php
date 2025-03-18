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

$sql = "SELECT username FROM userstable WHERE username = '". $loginUser . "'";
$result = $conn->query($sql);

if ($result->num_rows > 0) {
    //tell user that name is already exist 
    echo "Username is already taken.";

} else {
  echo "Creating user...";
  //Insert the user and pass into the database
  
    $sql2 = "INSERT INTO userstable (username, password)
    VALUES ('". $loginUser . "', '". $loginPass . "')";

    if ($conn->query($sql2) === TRUE) {
        echo "New record created successfully";
    } else {
        echo "Error: " . $sql2 . "<br>" . $conn->error;
    }
}
$conn->close();

?>