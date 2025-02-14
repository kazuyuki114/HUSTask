/** @type {import('tailwindcss').Config} */
export default {
  content: ['./index.html', './src/**/*.{vue,js,ts,jsx,tsx}'],
  theme: {
    extend: {
      fontSize: {
        '12xl': '9rem',
        '14xl': '10rem',
      },
      fontFamily: {
        sans: ['Poppins', 'sans-serif'],
      },
      gridTemplateColumns: {
        '70/30': '70% 28%',
      },
      colors: {
        "theme-primary": "#3B82F6",
        "theme-secondary": "#FA5757",
        "theme-grayish-blue": "#9194A1",
        "theme-dark-blue": "rgb(37, 43, 70)",
        "theme-dark-blue-tp": "rgba(37, 43, 70, 0.9)",
      },
    },
  },

  variants: {
    extend: {},
  },
  plugins: [],
};
