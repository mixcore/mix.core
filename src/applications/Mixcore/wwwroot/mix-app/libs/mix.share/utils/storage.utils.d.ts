export declare class LocalStorage {
    static setItem(key: string, value: string): void;
    static getItem(key: string): string | null;
    static removeItem(key: string): void;
    static clear(): void;
}
