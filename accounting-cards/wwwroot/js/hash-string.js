function toSHA256(password, salt) {
    let hash = `salt=${salt}+password=${password}`
    return CryptoJS.SHA256(hash).toString().toUpperCase();
}