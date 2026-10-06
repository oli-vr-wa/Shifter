interface FormatCurrencyOptions {
    locale?: string;
    currency?: string;
}

export function formatCurrency(amount: number, options?: FormatCurrencyOptions) {
    const { locale = 'en-AU', currency = 'AUD' } = options || {};
    return new Intl.NumberFormat(locale, { style: 'currency', currency }).format(amount);
}