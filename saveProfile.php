<?php
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
$data = json_decode(file_get_contents('php://input'), true);
$userId = (int)$data['user_id'];
$answers = $data['answers'];

// Profil speichern oder aktualisieren
if (isset($data['profile_id']) && $data['profile_id'] > 0) {
    // Vorhandenes Profil aktualisieren
    $profileId = (int)$data['profile_id'];
    $query = "UPDATE profiles SET modified_at = NOW()";
    
    for ($i = 0; $i < 36; $i++) {
        $query .= ", item" . ($i + 1) . " = " . (int)$answers[$i];
    }
    
    $query .= " WHERE id = ? AND user_id = ?";
    $stmt = $conn->prepare($query);
    $stmt->bind_param("ii", $profileId, $userId);
} else {
    // Neues Profil erstellen
    $query = "INSERT INTO profiles (user_id, created_at, modified_at";
    $values = "VALUES (?, NOW(), NOW()";
    
    for ($i = 0; $i < 36; $i++) {
        $query .= ", item" . ($i + 1);
        $values .= ", ?";
    }
    
    $query .= ") " . $values . ")";
    $stmt = $conn->prepare($query);
    
    $params = array_merge([$userId], $answers);
    $types = str_repeat("i", 37); // user_id + 36 items
    $stmt->bind_param($types, ...$params);
}

if ($stmt->execute()) {
    $profileId = isset($profileId) ? $profileId : $stmt->insert_id;
    $response = [
        "status" => "success",
        "profile_id" => $profileId
    ];
} else {
    $response = [
        "status" => "error",
        "message" => "Profil konnte nicht gespeichert werden"
    ];
}

$stmt->close();
$conn->close();

echo json_encode($response);
?>