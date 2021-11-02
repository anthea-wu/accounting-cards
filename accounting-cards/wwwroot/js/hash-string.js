function toSHA256(password) {
    let hash = `now=${Date.now()}+password=${password}`
    return sha256(hash).toUpperCase();
}