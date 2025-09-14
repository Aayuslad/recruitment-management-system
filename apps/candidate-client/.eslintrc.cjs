module.exports = {
  parser: '@typescript-eslint/parser',
  parserOptions: {
    ecmaVersion: 'latest',
    sourceType: 'module',
    ecmaFeatures: { jsx: true },
  },
  settings: {
    react: { version: 'detect' },
  },
  plugins: ['react', 'react-hooks', '@typescript-eslint', 'jsx-a11y'],
  extends: [
    'eslint:recommended',
    'plugin:react/recommended',
    'plugin:@typescript-eslint/recommended',
    'plugin:react-hooks/recommended',
    'plugin:jsx-a11y/recommended',
    'prettier', // disable ESLint rules that conflict with Prettier
  ],
  rules: {
    'react/react-in-jsx-scope': 'off', // not needed in React 17+
    'no-unused-vars': 'warn', // warn if variable declared but not used
    eqeqeq: ['error', 'always'], // force === instead of ==
    quotes: ['error', 'single'], // enforce single quotes
    semi: ['error', 'always'], // always use semicolons
    indent: ['error', 2], // 2 spaces indentation
    'react/prop-types': 'off', // not needed for TS
    '@typescript-eslint/no-explicit-any': 'warn', // avoid overusing "any"
  },
};
