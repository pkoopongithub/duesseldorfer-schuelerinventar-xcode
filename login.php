<?php
// Datenbankverbindung
$dbhost = "localhost";
$dbname = "personality_test";
$dbuser = "db_user";
$dbpass = "secure_password";

$conn = new mysqli($dbhost, $dbuser, $dbpass, $dbname);

$username = $_POST['username'];
$password = $_POST['password'];

$stmt = $conn->prepare("SELECT id, password FROM users WHERE username = ?");
$stmt->bind_param("s", $username);
$stmt->execute();
$result = $stmt->get_result();

if ($result->num_rows > 0) {
    $user = $result->fetch_assoc();
    if (password_verify($password, $user['password'])) {
        echo json_encode(['status' => 'success', 'user_id' => $user['id']]);
    } else {
        echo json_encode(['status' => 'error', 'message' => 'Invalid password']);
    }
} else {
    echo json_encode(['status' => 'error', 'message' => 'User not found']);
}

$conn->close();
?><?php
header('Content-Type: application/json');

// Datenbankkonfiguration
$dbhost = "localhost";
$dbname = "personality_test";
$dbuser = "db_user";
$dbpass = "secure_password";

// Verbindung herstellen
$conn = new mysqli($dbhost, $dbuser, $dbpass, $dbname);

if ($conn->connect_error) {
    die(json_encode(["status" => "error", "message" => "Datenbankverbindung fehlgeschlagen"]));
}

// Eingaben erhalten
$username = $conn->real_escape_string($_GET['username']);
$password = $_GET['password'];

// Benutzer abfragen
$stmt = $conn->prepare("SELECT id, username, email, password_hash, is_admin FROM users WHERE username = ?");
$stmt->bind_param("s", $username);
$stmt->execute();
$result = $stmt->get_result();

if ($result->num_rows === 0) {
    echo json_encode(["status" => "error", "message" => "Benutzer nicht gefunden"]);
    exit;
}

$user = $result->fetch_assoc();

// Passwort überprüfen
if (password_verify($password, $user['password_hash'])) {
    // Login erfolgreich
    $response = [
        "status" => "success",
        "user_id" => $user['id'],
        "username" => $user['username'],
        "email" => $user['email'],
        "is_admin" => (bool)$user['is_admin']
    ];
    
    // Letzten Login aktualisieren
    $updateStmt = $conn->prepare("UPDATE users SET last_login = NOW() WHERE id = ?");
    $updateStmt->bind_param("i", $user['id']);
    $updateStmt->execute();
} else {
    $response = ["status" => "error", "message" => "Falsches Passwort"];
}

$stmt->close();
$conn->close();

echo json_encode($response);
?>