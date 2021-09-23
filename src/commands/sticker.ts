import Sticker from '../db/datatype/sticker';

export const returnSticker = async (stickerName: string): Promise<string> => {
    const sticker = await Sticker.findOne({ where: { keyword: stickerName.toLowerCase() } });
    if (sticker == null) return `Sticker "${stickerName}" does not exist.`;
    return (sticker as any).uri as string;
}