const paths = {
  home: 'M3 10.8 12 3l9 7.8V21a1 1 0 0 1-1 1h-5v-7H9v7H4a1 1 0 0 1-1-1V10.8Z',
  users: 'M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2M9 11a4 4 0 1 0 0-8 4 4 0 0 0 0 8Zm13 10v-2a4 4 0 0 0-3-3.87M16 3.13a4 4 0 0 1 0 7.75',
  handshake: 'm8 11 2 2 4-4M12 15l2 2 4-4M3 8l4-4 5 2 5-2 4 4-3 7-4 4-7-7-4-4Z',
  badge: 'M12 15a4 4 0 1 0 0-8 4 4 0 0 0 0 8Zm-7 7a7 7 0 0 1 14 0M5 3h14v18H5z',
  shield: 'M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10Zm-3-10 2 2 4-4',
  materials: 'm12 2 9 5-9 5-9-5 9-5Zm-9 10 9 5 9-5M3 17l9 5 9-5',
  box: 'm21 8-9 5-9-5 9-5 9 5Zm-18 0v9l9 5 9-5V8M12 13v9',
  cart: 'M3 3h2l2 13h10l3-9H6M9 21a1 1 0 1 0 0-2 1 1 0 0 0 0 2Zm8 0a1 1 0 1 0 0-2 1 1 0 0 0 0 2Z',
  sale: 'M12 2v20m5-16H9.5a3.5 3.5 0 0 0 0 7h5a3.5 3.5 0 0 1 0 7H6',
  factory: 'M3 22V10l6-4v4l6-4v4l6-4v16H3Zm4-7h2m3 0h2m3 0h2',
  stock: 'M4 4h16v16H4zM4 9h16M9 4v16',
  cash: 'M3 6h18v12H3zM7 10H5m14 4h-2m-5 2a4 4 0 1 0 0-8 4 4 0 0 0 0 8Z',
  payable: 'M12 3v18m-5-5 5 5 5-5M5 5h14',
  receivable: 'M12 21V3m-5 5 5-5 5 5M5 19h14',
  pin: 'M20 10c0 5-8 12-8 12S4 15 4 10a8 8 0 1 1 16 0Zm-8 3a3 3 0 1 0 0-6 3 3 0 0 0 0 6Z',
  menu: 'M4 6h16M4 12h16M4 18h16',
  logout: 'M10 17l5-5-5-5m5 5H3m12-9h5a1 1 0 0 1 1 1v16a1 1 0 0 1-1 1h-5',
  search: 'm21 21-4.35-4.35M19 11a8 8 0 1 1-16 0 8 8 0 0 1 16 0Z',
  plus: 'M12 5v14M5 12h14',
  edit: 'M12 20h9M16.5 3.5a2.12 2.12 0 0 1 3 3L8 18l-4 1 1-4L16.5 3.5Z',
  eye: 'M2 12s3.5-7 10-7 10 7 10 7-3.5 7-10 7S2 12 2 12Zm10 3a3 3 0 1 0 0-6 3 3 0 0 0 0 6Z',
  trash: 'M3 6h18M8 6V4h8v2m3 0-1 15H6L5 6m5 4v7m4-7v7',
  close: 'M6 6l12 12M18 6 6 18',
  save: 'M5 3h12l2 2v16H5V3Zm3 0v6h8V3m-8 18v-7h8v7',
  chevron: 'm9 18 6-6-6-6',
}

export default function Icon({ name, size = 24, className = '' }) {
  return (
    <svg className={`icon ${className}`} width={size} height={size} viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round" aria-hidden="true">
      <path d={paths[name] || paths.box} />
    </svg>
  )
}
