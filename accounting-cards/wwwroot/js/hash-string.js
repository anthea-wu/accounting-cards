function toSHA256(password, salt) {
    let hash = `salt=${salt}+password=${password}`
    console.log(sha256(hash).toUpperCase());
    return sha256(hash).toUpperCase();
}