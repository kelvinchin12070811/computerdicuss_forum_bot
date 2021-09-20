export const returnSticker = (stickerName: string): string => {
    switch (stickerName) {
        case 'rick':
            return 'https://i.imgur.com/NQinKJB.mp4';
        case 'sv':
            return 'https://media.discordapp.net/attachments/774174877661134889/889204221184995399/unknown.png';

        case 'bread':
            return 'https://media.discordapp.net/attachments/833330336708165652/886266581464793178/Bread.png';
        default:
            return `No sticker called "${stickerName}"`;
    }
}