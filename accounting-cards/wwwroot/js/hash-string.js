function toSHA256(password, salt) {
    let hash = `salt=${salt}+password=${password}`
    return sha256(hash).toUpperCase();
}